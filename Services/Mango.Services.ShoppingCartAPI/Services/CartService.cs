using AutoMapper;
using Mango.Services.ShoppingCartAPI.Accessors.Interfaces;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Api;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;
using Shared.Exceptions;
using Shared.Message.Messages;
using Shared.Message.Services.Interfaces;
using Shared.Models.OperationResults;

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

        public async Task<CartApi> Get()
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                userCart = CreateNewUserCart();

            var userCartDto = _mapper.Map<CartApi>(userCart);
            return userCartDto;
        }

        public async Task<CartApi> AddItems(List<CartItemApi> cartItemsApi)
        {
            var userCart = await GetUserCart();
            if (userCart == null)
            {
                userCart = CreateNewUserCart();

                var cartItems = await CreateCartItems(cartItemsApi);
                userCart.CartItems = cartItems;

                await _cartRepository.Add(userCart);
            }
            else
            {
                var existingCartItems = userCart.CartItems?.ToList();
                if (existingCartItems == null || !existingCartItems.Any())
                {
                    var cartItems = await CreateCartItems(cartItemsApi);
                    userCart.CartItems = cartItems;
                }
                else
                {
                    var existingCartItemsByProduct = GetExistingCartItemsByProduct(existingCartItems, cartItemsApi.Select(x => x.Product.PublicId).ToList());
                    UpdateExistingCartItemsCount(existingCartItemsByProduct, cartItemsApi);

                    var nonExistingCartItemsByProduct = GetNonExistingCartItemsByProduct(cartItemsApi, existingCartItems.Select(x => x.Product.PublicId).ToList());
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

            return _mapper.Map<CartApi>(userCart);
        }

        public async Task<CartApi> UpdateItems(List<CartItemApi> cartItemsApi)
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                throw new NotFoundException("User cart not found");

            var existingCartItems = userCart.CartItems?.ToList();
            if (existingCartItems == null || !existingCartItems.Any())
            {
                var cartItems = await CreateCartItems(cartItemsApi);
                userCart.CartItems = cartItems;
            }
            else
            {
                foreach (var cartItemDto in cartItemsApi)
                {
                    var existingCartItem = existingCartItems.FirstOrDefault(x => x.PublicId == cartItemDto.PublicId);
                    if (existingCartItem != null)
                    {
                        existingCartItem.Count = cartItemsApi.Count;
                    }
                }
            }

            await _cartRepository.Update(userCart);

            return _mapper.Map<CartApi>(userCart);
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

        public async Task<bool> Checkout(CheckoutApi checkout)
        {
            var userCart = await Get();
            if (userCart == null || userCart.PublicId == Guid.Empty)
                throw new NotFoundException("User cart not found");

            var excessItems = userCart.CartItems.Select(x => x.PublicId).Except(checkout.CartItems.Select(x => x.PublicId));
            if (excessItems.Any())
                throw new ValidationErrorException(new ValidationMessage { Field = nameof(checkout.CartItems), Message = "Incorrect items in the request" });

            var checkoutMessage = _mapper.Map<CheckoutMessage>(checkout);
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

            return userCart ?? new Cart();
        }

        private async Task<List<CartItem>> CreateCartItems(List<CartItemApi> cartItemsDto)
        {
            var incomeCartProductPublicIds = GetIncomeCartProductPublicIds(cartItemsDto);
            var existingCartProducts = await GetExistingCartProducts(incomeCartProductPublicIds);
            var publicIdToIdProjection = GetPublicIdToIdProjection(existingCartProducts);

            var cartItems = cartItemsDto?.Select(x => CreateCartItem(x, publicIdToIdProjection)).ToList();

            return cartItems ?? new List<CartItem>();
        }

        private async Task<List<CartProduct>> GetExistingCartProducts(List<Guid>? cartProductPublicIds = null)
        {
            var existingCartProductsBaseQuery = _cartProductRepository.Query();

            if (cartProductPublicIds != null && cartProductPublicIds.Any())
                existingCartProductsBaseQuery = existingCartProductsBaseQuery.Where(x => cartProductPublicIds.Contains(x.PublicId));

            var existingCartProducts = await existingCartProductsBaseQuery.ToListAsync();

            return existingCartProducts;
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

        private static List<Guid> GetIncomeCartProductPublicIds(List<CartItemApi> cartItemsDto)
        {
            var incomeCartProductPublicIds = cartItemsDto
                   .Where(x => x.Product != null && x.Product.PublicId != Guid.Empty)
                   .Select(x => x.Product.PublicId)
                   .ToList();

            return incomeCartProductPublicIds;
        }

        private static Dictionary<Guid, int> GetPublicIdToIdProjection(List<CartProduct> cartProducts)
        {
            return cartProducts.ToDictionary(key => key.PublicId, value => value.Id);
        }

        private static CartItem CreateCartItem(CartItemApi cartItemDto, Dictionary<Guid, int> existedProductIds)
        {
            var cartItem = new CartItem
            {
                Count = cartItemDto.Count,
            };

            if (cartItemDto.Product.PublicId != Guid.Empty && existedProductIds.ContainsKey(cartItemDto.Product.PublicId))
            {
                cartItem.ProductId = existedProductIds[cartItemDto.Product.PublicId];
            }
            else
            {
                cartItem.Product = new CartProduct
                {
                    PublicId = cartItemDto.Product.PublicId,
                    Name = cartItemDto.Product.Name,
                    Price = cartItemDto.Product.Price,
                    CategoryName = cartItemDto.Product.CategoryName,
                    Description = cartItemDto.Product.Description,
                    ImageUrl = cartItemDto.Product.ImageUrl,
                };
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

        private static void UpdateExistingCartItemsCount(List<CartItem> existingCartItemsByProduct, List<CartItemApi> cartItemsDto)
        {
            foreach (var existingCartItemByProduct in existingCartItemsByProduct)
            {
                var additionalCount = cartItemsDto.FirstOrDefault(incomeCartItem => incomeCartItem.Product.PublicId == existingCartItemByProduct.Product.PublicId)?.Count ?? 0;
                existingCartItemByProduct.Count += additionalCount;
            }
        }

        private static List<CartItemApi> GetNonExistingCartItemsByProduct(List<CartItemApi> allIncomeCartItems, List<Guid> productPublicIds)
        {
            var nonExistingCartItemsByProduct = allIncomeCartItems
                .Where(incomeCartItem => !productPublicIds.Any(productPublicId => incomeCartItem.Product.PublicId == productPublicId))
                .ToList();

            return nonExistingCartItemsByProduct;
        }
    }
}
