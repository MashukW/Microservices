using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Responses;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart-items")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet("{cartId}")]
        public async Task<ResponseData<IList<CartItemDto>>> GetItems(Guid cartId)
        {
            try
            {
                var cartItemsDto = await _cartItemService.GetItems(cartId);
                return cartItemsDto;
            }
            catch (Exception ex)
            {
                return ResponseData<IList<CartItemDto>>.Fail(ex.Message);
            }
        }

        [HttpPost("{cartId}")]
        public async Task<ResponseData<IList<CartItemDto>>> AddUpdateItems(Guid cartId, [FromBody] List<CartItemDto> cartItems)
        {
            try
            {
                var cartItemsDto = await _cartItemService.AddUpdateItems(cartId, cartItems);
                return cartItemsDto;
            }
            catch (Exception ex)
            {
                return ResponseData<IList<CartItemDto>>.Fail(ex.Message);
            }
        }

        [HttpDelete("{cartId}")]
        public async Task<ResponseData<bool>> RemoveItems(Guid cartId, [FromBody] List<Guid> cartItemIds)
        {
            try
            {
                var cartItemsDto = await _cartItemService.RemoveItems(cartId, cartItemIds);
                return cartItemsDto;
            }
            catch (Exception ex)
            {
                return ResponseData<bool>.Fail(ex.Message);
            }
        }
    }
}
