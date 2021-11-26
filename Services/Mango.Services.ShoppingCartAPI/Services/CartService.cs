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

        public async Task<Result<CartDto>> Get(Guid userId)
        {
            var userCart = await _cartRepository
                .Query()
                .Include(x => x.CartItems).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserPublicId == userId);

            if (userCart == null)
            {
                return Result.NotFound("cart not found");
            }

            var userCartDto = _mapper.Map<CartDto>(userCart);
            return userCartDto;
        }

        public async Task<Result<CartDto>> Create(CartDto cartDto)
        {
            var validationResult = await ValidateCart(cartDto);
            if (validationResult.IsSuccess == false)
                return Result.ValidationError(validationResult.Failure.ValidationMessages.ToArray());

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

        public async Task<Result<bool>> Clear(Guid cartId)
        {
            var userCart = await _cartRepository.Query().Include(x => x.CartItems).FirstOrDefaultAsync(x => x.PublicId == cartId);
            if (userCart == null)
                return false;

            userCart.CartItems?.Clear();

            await _cartRepository.Update(userCart);

            await _workUnit.SaveChanges();

            return true;
        }

        private async Task<Result<bool>> ValidateCart(CartDto cartDto)
        {
            var userCart = await _cartRepository.Query(x => x.UserPublicId == cartDto.UserPublicId).FirstOrDefaultAsync();
            if (userCart != null)
            {
                return Result.ValidationError(new ValidationMessage { Field = "Cart", Message = "Cart already exists" });
            }

            if (cartDto.UserPublicId == Guid.Empty)
            {
                return Result.ValidationError(new ValidationMessage { Field = nameof(cartDto.UserPublicId), Message = "User id can not be empty" });
            }

            var cartItemWithCountLessOrEqualToZero = cartDto?.CartItems?.Where(x => x.Count <= 0).ToList();
            if (cartItemWithCountLessOrEqualToZero != null && cartItemWithCountLessOrEqualToZero.Any())
            {
                var validationErrors = cartItemWithCountLessOrEqualToZero
                    .Select(x => new ValidationMessage
                    {
                        Field = nameof(CartItem.Count),
                        Message = $"Cart item entity {x.PublicId} has no count"
                    })
                    .ToArray();

                return Result.ValidationError(validationErrors);
            }

            var cartItemWithNullProduct = cartDto?.CartItems?.Where(x => x.Product == null).ToList();
            if (cartItemWithNullProduct != null && cartItemWithNullProduct.Any())
            {
                var validationErrors = cartItemWithNullProduct
                    .Select(x => new ValidationMessage
                    {
                        Field = nameof(CartItem.Product),
                        Message = $"Cart item entity {x.PublicId} has no product"
                    })
                    .ToList();

                return Result.ValidationError(validationErrors);
            }

            var cartItemWithEmptyProductId = cartDto?.CartItems?.Where(x => x.Product != null && x.Product.PublicId == Guid.Empty).ToList();
            if (cartItemWithEmptyProductId != null && cartItemWithEmptyProductId.Any())
            {
                var validationErrors = cartItemWithEmptyProductId
                    .Select(x => new ValidationMessage
                    {
                        Field = nameof(CartItem.Product),
                        Message = $"Cart item entity {x.PublicId} has empty product"
                    })
                    .ToList();

                return Result.ValidationError(validationErrors);
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
