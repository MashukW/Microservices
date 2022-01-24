using AutoMapper;
using Mango.Services.ShoppingCartAPI.Accessors.Interfaces;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Api;
using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Messages;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;
using Shared.Exceptions;
using Shared.Message.Services.Interfaces;
using Shared.Models;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CartService : ICartService
    {
        private readonly IUserAccessor _userAccessor;

        private readonly IMessageBus _messageBus;

        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<CartProduct> _cartProductRepository;

        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public CartService(
            IUserAccessor userAccessor,
            IMessageBus messageBus,
            IRepository<Cart> cartRepository,
            IRepository<CartProduct> cartProductRepository,
            IWorkUnit workUnit,
            IMapper mapper)
        {
            _userAccessor = userAccessor;

            _messageBus = messageBus;

            _cartRepository = cartRepository;
            _cartProductRepository = cartProductRepository;

            _workUnit = workUnit;
            _mapper = mapper;
        }

        public async Task<CartOutgoing> Get()
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                userCart = CreateNewUserCart();

            var userCartDto = _mapper.Map<CartOutgoing>(userCart);
            return userCartDto;
        }

        public async Task<CartOutgoing> AddItems(List<CartItemIncoming> incomingCartItems)
        {
            var userCart = await GetUserCart();
            if (userCart == null)
            {
                userCart = CreateNewUserCart();

                var cartItems = await CreateCartItems(incomingCartItems);
                userCart.CartItems = cartItems;

                await _cartRepository.Add(userCart);
            }
            else
            {
                var existingCartItems = userCart.CartItems?.ToList();
                if (existingCartItems == null || !existingCartItems.Any())
                {
                    var cartItems = await CreateCartItems(incomingCartItems);
                    userCart.CartItems = cartItems;
                }
                else
                {
                    var existingCartItemsByProduct = GetExistingCartItemsByProduct(existingCartItems, incomingCartItems.Select(x => x.Product.PublicId).ToList());
                    UpdateExistingCartItemsCount(existingCartItemsByProduct, incomingCartItems);

                    var nonExistingCartItemsByProduct = GetNonExistingCartItemsByProduct(incomingCartItems, existingCartItems.Select(x => x.Product.PublicId).ToList());
                    if (nonExistingCartItemsByProduct != null && nonExistingCartItemsByProduct.Any())
                    {
                        var newCartItems = await CreateCartItems(nonExistingCartItemsByProduct);
                        foreach (var newCartItem in newCartItems)
                        {
                            userCart.CartItems?.Add(newCartItem);
                        }
                    }
                }

                await _cartRepository.Update(userCart);
            }

            await _workUnit.SaveChanges();

            return _mapper.Map<CartOutgoing>(userCart);
        }

        public async Task<CartOutgoing> UpdateItems(List<CartItemIncoming> incomingCartItems)
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                throw new NotFoundException("User cart not found");

            var existingCartItems = userCart.CartItems?.ToList();
            if (existingCartItems == null || !existingCartItems.Any())
            {
                var cartItems = await CreateCartItems(incomingCartItems);
                userCart.CartItems = cartItems;
            }
            else
            {
                foreach (var cartItemDto in incomingCartItems)
                {
                    var existingCartItem = existingCartItems.FirstOrDefault(x => x.PublicId == cartItemDto.PublicId);
                    if (existingCartItem != null)
                    {
                        existingCartItem.Count = incomingCartItems.Count;
                    }
                }
            }

            await _cartRepository.Update(userCart);

            return _mapper.Map<CartOutgoing>(userCart);
        }

        public async Task<bool> RemoveItems(List<Guid> cartItemsPublicIds)
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                throw new NotFoundException("User cart not found");

            var cartItemsForRemoving = userCart.CartItems?
                .Where(x => cartItemsPublicIds.Contains(x.PublicId))
                .ToList();

            if (cartItemsForRemoving != null && cartItemsForRemoving.Any())
            {
                foreach (var cartItemForRemoving in cartItemsForRemoving)
                    userCart.CartItems?.Remove(cartItemForRemoving);

                if (userCart.CartItems == null || userCart.CartItems.Count == 0)
                    userCart.CouponCode = string.Empty;

                await _cartRepository.Update(userCart);
                await _workUnit.SaveChanges();

                return true;
            }

            return false;
        }

        public async Task<bool> ApplyCoupon(string couponCode)
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                throw new NotFoundException("User cart not found");

            userCart.CouponCode = couponCode;
            await _cartRepository.Update(userCart);
            await _workUnit.SaveChanges();

            return true;
        }

        public async Task<bool> RemoveCoupon()
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                throw new NotFoundException("User cart not found");

            userCart.CouponCode = "";
            await _cartRepository.Update(userCart);
            await _workUnit.SaveChanges();

            return true;
        }

        public async Task<bool> Checkout(CheckoutIncoming incomingCheckout)
        {
            var userCart = await Get();
            if (userCart == null || userCart.PublicId == Guid.Empty)
                throw new NotFoundException("User cart not found");

            var excessItems = userCart.CartItems.Select(x => x.PublicId).Except(incomingCheckout.CartItems.Select(x => x.PublicId));
            if (excessItems.Any())
                throw new ValidationErrorException(new ValidationMessage { Field = nameof(incomingCheckout.CartItems), Messages = new List<string> { "Incorrect items in the request" } });

            var checkoutMessage = _mapper.Map<CheckoutMessage>(incomingCheckout);
            await _messageBus.Publish(checkoutMessage, "checkout");

            return true;
        }

        public async Task<bool> Clear()
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                throw new NotFoundException("User cart not found");

            userCart.CartItems?.Clear();
            userCart.CouponCode = string.Empty;

            await _cartRepository.Update(userCart);

            await _workUnit.SaveChanges();

            return true;
        }

        private async Task<Cart> GetUserCart()
        {
            var user = _userAccessor.GetAppUser();

            var userCart = await _cartRepository
                .Query()
                .Include(x => x.CartItems).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserPublicId == user.Id);

            return userCart ?? throw new NotFoundException("User cart not found");
        }

        private Cart CreateNewUserCart()
        {
            var user = _userAccessor.GetAppUser();

            return new Cart
            {
                UserPublicId = user.Id,
                CouponCode = string.Empty
            };
        }

        private async Task<List<CartProduct>> GetExistingCartProducts(List<Guid>? cartProductPublicIds = null)
        {
            var existingCartProductsBaseQuery = _cartProductRepository.Query();

            if (cartProductPublicIds != null && cartProductPublicIds.Any())
                existingCartProductsBaseQuery = existingCartProductsBaseQuery.Where(x => cartProductPublicIds.Contains(x.PublicId));

            var existingCartProducts = await existingCartProductsBaseQuery.ToListAsync();

            return existingCartProducts;
        }

        private static Dictionary<Guid, int> CreatePublicIdToIdProjection(List<CartProduct> cartProducts)
        {
            return cartProducts.ToDictionary(key => key.PublicId, value => value.Id);
        }

        private async Task<List<CartItem>> CreateCartItems(List<CartItemIncoming> incomingCartItems)
        {
            var incomeCartProductPublicIds = incomingCartItems
                .Where(x => x.Product != null && x.Product.PublicId != Guid.Empty)
                .Select(x => x.Product.PublicId)
                .ToList();

            var existingCartProducts = await GetExistingCartProducts(incomeCartProductPublicIds);
            var publicIdToIdProjection = CreatePublicIdToIdProjection(existingCartProducts);

            var cartItems = incomingCartItems?.Select(incomingCartItem => CreateCartItem(incomingCartItem, publicIdToIdProjection)).ToList();

            return cartItems ?? new List<CartItem>();
        }

        private CartItem CreateCartItem(CartItemIncoming incomingCartItem, Dictionary<Guid, int> existingProductIds)
        {
            var cartItem = new CartItem
            {
                Count = incomingCartItem.Count,
            };

            if (incomingCartItem.Product.PublicId != Guid.Empty && existingProductIds.ContainsKey(incomingCartItem.Product.PublicId))
            {
                cartItem.ProductId = existingProductIds[incomingCartItem.Product.PublicId];
            }
            else
            {
                var product = _mapper.Map<CartProduct>(incomingCartItem.Product);
                cartItem.Product = product;
            }

            return cartItem;
        }

        private static List<CartItem> GetExistingCartItemsByProduct(List<CartItem> allExistingCartItems, List<Guid> productPublicIds)
        {
            var existingCartItemsByProduct = allExistingCartItems
                .Where(existringCartItem => productPublicIds.Any(productPublicId => existringCartItem.Product.PublicId == productPublicId))
                .ToList();

            return existingCartItemsByProduct;
        }

        private static List<CartItemIncoming> GetNonExistingCartItemsByProduct(List<CartItemIncoming> incomingCartItems, List<Guid> productPublicIds)
        {
            var nonExistingCartItemsByProduct = incomingCartItems
                .Where(incomeCartItem => !productPublicIds.Any(productPublicId => incomeCartItem.Product.PublicId == productPublicId))
                .ToList();

            return nonExistingCartItemsByProduct;
        }

        private static void UpdateExistingCartItemsCount(List<CartItem> existingCartItems, List<CartItemIncoming> incomingCartItems)
        {
            foreach (var existingCartItem in existingCartItems)
            {
                var incomingCartItem = incomingCartItems.FirstOrDefault(incomeCartItem => incomeCartItem.Product.PublicId == existingCartItem.Product.PublicId);
                if (incomingCartItem != null)
                {
                    existingCartItem.Count += incomingCartItem.Count;
                }
            }
        }
    }
}
