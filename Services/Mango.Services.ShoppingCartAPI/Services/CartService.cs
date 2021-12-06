using AutoMapper;
using Mango.Services.ShoppingCartAPI.Accessors.Interfaces;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;
using Shared.Models.OperationResults;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CartService : ICartService
    {
        private readonly IUserAccessor _userAccessor;

        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<CartProduct> _cartProductRepository;

        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public CartService(
            IUserAccessor userAccessor,
            IRepository<Cart> cartRepository,
            IRepository<CartProduct> cartProductRepository,
            IWorkUnit workUnit,
            IMapper mapper)
        {
            _userAccessor = userAccessor;

            _cartRepository = cartRepository;
            _cartProductRepository = cartProductRepository;

            _workUnit = workUnit;
            _mapper = mapper;
        }

        public async Task<Result<CartDto>> Get()
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                userCart = CreateNewUserCart();

            var userCartDto = _mapper.Map<CartDto>(userCart);
            return userCartDto;
        }

        public async Task<Result<CartDto>> AddItems(List<CartItemDto> cartItemsDto)
        {
            var userCart = await GetUserCart();
            if (userCart == null)
            {
                userCart = CreateNewUserCart();

                var cartItems = await CreateCartItems(cartItemsDto);
                userCart.CartItems = cartItems;

                await _cartRepository.Add(userCart);
            }
            else
            {
                var existingCartItems = userCart.CartItems?.ToList();
                if (existingCartItems == null || !existingCartItems.Any())
                {
                    var cartItems = await CreateCartItems(cartItemsDto);
                    userCart.CartItems = cartItems;
                }
                else
                {
                    var existingCartItemsByProduct = GetExistingCartItemsByProduct(existingCartItems, cartItemsDto.Select(x => x.Product.PublicId).ToList());
                    UpdateExistingCartItemsCount(existingCartItemsByProduct, cartItemsDto);

                    var nonExistingCartItemsByProduct = GetNonExistingCartItemsByProduct(cartItemsDto, existingCartItems.Select(x => x.Product.PublicId).ToList());
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

            return _mapper.Map<CartDto>(userCart);
        }

        public async Task<Result<CartDto>> UpdateItems(List<CartItemDto> cartItemsDto)
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                return Result.NotFound("User cart not found");

            var existingCartItems = userCart.CartItems?.ToList();
            if (existingCartItems == null || !existingCartItems.Any())
            {
                var cartItems = await CreateCartItems(cartItemsDto);
                userCart.CartItems = cartItems;
            }
            else
            {
                foreach (var cartItemDto in cartItemsDto)
                {
                    var existingCartItem = existingCartItems.FirstOrDefault(x => x.PublicId == cartItemDto.PublicId);
                    if (existingCartItem != null)
                    {
                        existingCartItem.Count = cartItemDto.Count;
                    }
                }
            }

            await _cartRepository.Update(userCart);

            return _mapper.Map<CartDto>(userCart);
        }

        public async Task<Result<bool>> RemoveItems(List<Guid> cartItemsPublicIds)
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                return Result.NotFound("User cart not found");

            var cartItemsForRemoving = userCart.CartItems?
                .Where(x => cartItemsPublicIds.Contains(x.PublicId))
                .ToList();

            if (cartItemsForRemoving != null && cartItemsForRemoving.Any())
            {
                foreach (var cartItemForRemoving in cartItemsForRemoving)
                    userCart.CartItems?.Remove(cartItemForRemoving);

                await _cartRepository.Update(userCart);
                await _workUnit.SaveChanges();

                return true;
            }

            return false;
        }

        public async Task<Result<bool>> Clear()
        {
            var userCart = await GetUserCart();
            if (userCart == null)
                return Result.NotFound("User cart not found");

            userCart.CartItems?.Clear();

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

            return userCart;
        }

        private async Task<List<CartItem>> CreateCartItems(List<CartItemDto> cartItemsDto)
        {
            var incomeCartProductPublicIds = GetIncomeCartProductPublicIds(cartItemsDto);
            var existingCartProducts = await GetExistingCartProducts(incomeCartProductPublicIds);
            var publicIdToIdProjection = GetPublicIdToIdProjection(existingCartProducts);

            var cartItems = cartItemsDto?.Select(x => CreateCartItem(x, publicIdToIdProjection)).ToList();

            return cartItems;
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

        private static List<Guid> GetIncomeCartProductPublicIds(List<CartItemDto> cartItemsDto)
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

        private static CartItem CreateCartItem(CartItemDto cartItemDto, Dictionary<Guid, int> existedProductIds)
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

        private static void UpdateExistingCartItemsCount(List<CartItem> existingCartItemsByProduct, List<CartItemDto> cartItemsDto)
        {
            foreach (var existingCartItemByProduct in existingCartItemsByProduct)
            {
                var additionalCount = cartItemsDto.FirstOrDefault(incomeCartItem => incomeCartItem.Product.PublicId == existingCartItemByProduct.Product.PublicId)?.Count ?? 0;
                existingCartItemByProduct.Count += additionalCount;
            }
        }

        private static List<CartItemDto> GetNonExistingCartItemsByProduct(List<CartItemDto> allIncomeCartItems, List<Guid> productPublicIds)
        {
            var nonExistingCartItemsByProduct = allIncomeCartItems
                .Where(incomeCartItem => !productPublicIds.Any(productPublicId => incomeCartItem.Product.PublicId == productPublicId))
                .ToList();

            return nonExistingCartItemsByProduct;
        }
    }
}
