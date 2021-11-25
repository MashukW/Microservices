using AutoMapper;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;
using Shared.Models;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IRepository<CartProduct> _cartProductRepository;

        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public CartItemService(
            IRepository<Cart> cartRepository,
            IRepository<CartItem> cartItemRepository,
            IRepository<CartProduct> cartProductRepository,
            IWorkUnit workUnit,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _cartProductRepository = cartProductRepository;
            _workUnit = workUnit;
            _mapper = mapper;

        }

        public async Task<OperationResult<IList<CartItemDto>>> GetItems(Guid cartId)
        {
            var userCartItems = await _cartItemRepository
                .Query(x => x.Cart.PublicId == cartId)
                .Include(x => x.Product)
                .ToListAsync();

            return _mapper.Map<List<CartItemDto>>(userCartItems);
        }

        public async Task<OperationResult<IList<CartItemDto>>> AddUpdateItems(Guid cartId, IList<CartItemDto> cartItemsDto)
        {
            var userCart = await _cartRepository
                .Query()
                .Include(x => x.CartItems).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.PublicId == cartId);

            if (userCart == null)
            {
                return OperationResult<IList<CartItemDto>>.ValidationFail(new List<ValidationError>
                {
                    { new ValidationError { Property = nameof(cartId), Message = "Cart not found"} }
                });
            }

            var inputProductIds = cartItemsDto
                .Where(x => x.Product.PublicId != Guid.Empty)
                .Select(x => x.Product.PublicId)
                .ToList();

            var existedProductIds = await _cartProductRepository
                .Query()
                .Where(x => inputProductIds != null && inputProductIds.Contains(x.PublicId))
                .ToDictionaryAsync(key => key.PublicId, value => value.Id);

            foreach (var cartItemDto in cartItemsDto)
            {
                var productCartItem = userCart.CartItems?.FirstOrDefault(x => x.Product.PublicId == cartItemDto.Product.PublicId);
                if (productCartItem != null)
                {
                    UpdateCartItem(productCartItem, cartItemDto);
                }
                else
                {
                    var newCartItem = CreateNewCartItem(cartItemDto, existedProductIds);
                    userCart.CartItems?.Add(newCartItem);
                }
            }

            await _cartRepository.Update(userCart);
            await _workUnit.SaveChanges();

            return _mapper.Map<List<CartItemDto>>(userCart.CartItems);
        }

        public async Task<OperationResult<bool>> RemoveItems(Guid cartId, IList<Guid> cartItemIds)
        {
            var userCart = await _cartRepository
                .Query()
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.PublicId == cartId);

            if (userCart == null)
            {
                return OperationResult<bool>.ValidationFail(new List<ValidationError>
                {
                    { new ValidationError { Property = nameof(cartId), Message = "Cart not found"} }
                });
            }

            var itemsToRemove = userCart.CartItems?.Where(x => cartItemIds.Contains(x.PublicId)).ToList();
            if (itemsToRemove == null || !itemsToRemove.Any())
            {
                return false;
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                userCart.CartItems?.Remove(itemToRemove);
            }

            await _cartRepository.Update(userCart);
            await _workUnit.SaveChanges();

            return true;
        }

        private static void UpdateCartItem(CartItem productCartItem, CartItemDto cartItemDto)
        {
            productCartItem.Count = cartItemDto.Count;

            productCartItem.Product.Name = cartItemDto.Product.Name;
            productCartItem.Product.Price = cartItemDto.Product.Price;
            productCartItem.Product.Description = cartItemDto.Product.Description;
            productCartItem.Product.CategoryName = cartItemDto.Product.CategoryName;
            productCartItem.Product.ImageUrl = cartItemDto.Product.ImageUrl;
        }

        private static CartItem CreateNewCartItem(CartItemDto cartItemDto, Dictionary<Guid, int> existedProductIds)
        {
            var cartItem = new CartItem
            {
                Count = cartItemDto.Count,
            };

            if (existedProductIds.ContainsKey(cartItemDto.Product.PublicId))
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

