using AutoMapper;
using Mango.Services.ShoppingCartAPI.Accessors.Interfaces;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;
using Shared.Models;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<CartProduct> _cartProductRepository;

        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public CartService(
            IRepository<Cart> repository,
            IRepository<CartProduct> cartProductRepository,
            IUserAccessor userAccessor,
            IWorkUnit workUnit,
            IMapper mapper)
        {
            _cartRepository = repository;
            _cartProductRepository = cartProductRepository;
            _workUnit = workUnit;
            _mapper = mapper;
        }

        public async Task<OperationResult<CartDto>> Get(Guid userId)
        {
            var userCart = await _cartRepository
                .Query()
                .Include(x => x.CartItems).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserPublicId == userId);

            if (userCart == null)
            {
                return OperationResult<CartDto>.ValidationFail(new List<ValidationError>
                {
                    { new ValidationError { Property = nameof(userId), Message = "Cart not found"} }
                });
            }

            var userCartDto = _mapper.Map<CartDto>(userCart);
            return userCartDto;
        }

        public async Task<OperationResult<CartDto>> Create(CartDto cartDto)
        {
            var validationResult = await ValidateCart(cartDto);
            if (validationResult.Data == false)
                return OperationResult<CartDto>.ValidationFail(validationResult.ValidationErrors);

            var inputProductIds = cartDto?
                .CartItems?
                .Where(x => x.Product != null && x.Product.PublicId != Guid.Empty)
                .Select(x => x.Product.PublicId)
                .ToList();

            var existedProductIds = await _cartProductRepository
                .Query()
                .Where(x => inputProductIds != null && inputProductIds.Contains(x.PublicId))
                .ToDictionaryAsync(key => key.PublicId, value => value.Id);

            var userCart = new Cart()
            {
                UserPublicId = cartDto.UserPublicId,
                CouponCode = cartDto.CouponCode
            };

            var cartItems = cartDto?.CartItems?.Select(x => CreateCartItem(x, existedProductIds)).ToList();
            userCart.CartItems = cartItems;

            await _cartRepository.Add(userCart);
            await _workUnit.SaveChanges();

            return _mapper.Map<CartDto>(userCart);
        }

        public async Task<OperationResult<bool>> Clear(Guid cartId)
        {
            var userCart = await _cartRepository.Query().Include(x => x.CartItems).FirstOrDefaultAsync(x => x.PublicId == cartId);
            if (userCart == null)
                return false;

            userCart.CartItems?.Clear();

            await _cartRepository.Update(userCart);

            await _workUnit.SaveChanges();

            return true;
        }

        private async Task<OperationResult<bool>> ValidateCart(CartDto cartDto)
        {
            var userCart = await _cartRepository.Query(x => x.UserPublicId == cartDto.UserPublicId).FirstOrDefaultAsync();
            if (userCart != null)
            {
                return OperationResult<bool>.ValidationFail(new List<ValidationError>
                {
                    { new ValidationError { Property = "Cart", Message = "Cart already exists"} }
                });
            }

            if (cartDto.UserPublicId == Guid.Empty)
            {
                return OperationResult<bool>.ValidationFail(new List<ValidationError>
                {
                    { new ValidationError { Property = nameof(cartDto.UserPublicId), Message = "User id can not be empty"} }
                });
            }

            var cartItemWithCountLessOrEqualToZero = cartDto?.CartItems?.Where(x => x.Count <= 0).ToList();
            if (cartItemWithCountLessOrEqualToZero != null && cartItemWithCountLessOrEqualToZero.Any())
            {
                var validationErrors = cartItemWithCountLessOrEqualToZero
                    .Select(x => new ValidationError
                    {
                        Property = nameof(CartItem.Count),
                        Message = $"Cart item entity {x.PublicId} has no count"
                    })
                    .ToList();

                return OperationResult<bool>.ValidationFail(validationErrors);
            }

            var cartItemWithNullProduct = cartDto?.CartItems?.Where(x => x.Product == null).ToList();
            if (cartItemWithNullProduct != null && cartItemWithNullProduct.Any())
            {
                var validationErrors = cartItemWithNullProduct
                    .Select(x => new ValidationError
                    {
                        Property = nameof(CartItem.Product),
                        Message = $"Cart item entity {x.PublicId} has no product"
                    })
                    .ToList();

                return OperationResult<bool>.ValidationFail(validationErrors);
            }

            var cartItemWithEmptyProductId = cartDto?.CartItems?.Where(x => x.Product != null && x.Product.PublicId == Guid.Empty).ToList();
            if (cartItemWithEmptyProductId != null && cartItemWithEmptyProductId.Any())
            {
                var validationErrors = cartItemWithEmptyProductId
                    .Select(x => new ValidationError
                    {
                        Property = nameof(CartItem.Product),
                        Message = $"Cart item entity {x.PublicId} has empty product"
                    })
                    .ToList();

                return OperationResult<bool>.ValidationFail(validationErrors);
            }

            return true;
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
    }
}
