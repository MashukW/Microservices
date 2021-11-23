using AutoMapper;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IRepository<CartProduct> _cartProductRepository;

        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public CartService(
            IRepository<Cart> repository,
            IRepository<CartItem> cartItemRepository,
            IRepository<CartProduct> cartProductRepository,
            IWorkUnit workUnit,
            IMapper mapper)
        {
            _cartRepository = repository;
            _cartItemRepository = cartItemRepository;
            _cartProductRepository = cartProductRepository;
            _workUnit = workUnit;
            _mapper = mapper;

        }

        public async Task<CartDto> Get(Guid userId)
        {
            var userCart = await _cartRepository
                .Query()
                .Include(x => x.CartItems).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserPublicId == userId);

            var userCartDto = _mapper.Map<CartDto>(userCart);
            return userCartDto ?? new CartDto();
        }

        public async Task<CartDto> AddUpdate(CartDto cartDto)
        {
            var inputProductPublicIds = cartDto?.CartItems?.Where(x => x.Product != null && x.Product.PublicId != Guid.Empty).Select(x => x.Product.PublicId).ToList();
            var existedProductIds = await _cartProductRepository
                .Query()
                .Where(x => inputProductPublicIds != null && inputProductPublicIds.Contains(x.PublicId))
                .ToDictionaryAsync(key => key.PublicId, value => value.Id);

            var userCart = await _cartRepository.Query().Include(x => x.CartItems).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.UserPublicId == cartDto.UserPublicId);

            if (userCart == null)
            {
                userCart = new Cart()
                {
                    PublicId = Guid.NewGuid(),
                    UserPublicId = cartDto.UserPublicId,
                    CouponCode = cartDto.CouponCode,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                };

                var cartItems = cartDto?.CartItems?.Select(x => CreateCartItem(x, existedProductIds)).ToList();
                userCart.CartItems = cartItems;

                await _cartRepository.Add(userCart);
            }
            else
            {
                if (userCart == null)
                    return new CartDto();

                userCart.CouponCode = cartDto.CouponCode;
                userCart.DateUpdated = DateTime.UtcNow;

                var cartItems = cartDto?.CartItems?.Select(x => CreateCartItem(x, existedProductIds)).ToList();
                userCart.CartItems = cartItems;

                await _cartRepository.Update(userCart);
            }

            await _workUnit.SaveChanges();

            return _mapper.Map<CartDto>(userCart);
        }

        public async Task<bool> RemoveItem(Guid cartId, Guid cartItemId)
        {
            var cartItem = await _cartItemRepository.Query().FirstOrDefaultAsync(x => x.Cart.PublicId == cartId && x.PublicId == cartItemId);
            if (cartItem == null)
                return true;

            await _cartItemRepository.Remove(cartItem);
            await _workUnit.SaveChanges();

            return true;
        }

        public async Task<bool> Clear(Guid cartId)
        {
            var userCart = await _cartRepository.Query().Include(x => x.CartItems).FirstOrDefaultAsync(x => x.PublicId == cartId);
            if (userCart == null)
                return true;

            userCart.CartItems?.Clear();

            await _cartRepository.Update(userCart);

            await _workUnit.SaveChanges();

            return true;
        }

        private static CartItem CreateCartItem(CartItemDto cartItemDto, Dictionary<Guid, int> existedProductIds)
        {
            var cartItem = new CartItem
            {
                PublicId = cartItemDto.PublicId == Guid.Empty
                        ? Guid.NewGuid()
                        : cartItemDto.PublicId,

                Count = cartItemDto.Count,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            if (cartItemDto.Product.PublicId != Guid.Empty && existedProductIds.ContainsKey(cartItemDto.Product.PublicId))
            {
                cartItem.ProductId = existedProductIds[cartItemDto.Product.PublicId];
            }
            else
            {
                cartItem.Product = new CartProduct
                {
                    PublicId = Guid.NewGuid(),
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
