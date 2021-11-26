using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;
using Shared.Models.OperationResults;

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
        public async Task<ApiResult<IList<CartItemDto>>> GetItems(Guid cartId)
        {
            try
            {
                var cartItemsDto = await _cartItemService.GetItems(cartId);
                return cartItemsDto;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpPost("{cartId}")]
        public async Task<ApiResult<IList<CartItemDto>>> AddUpdateItems(Guid cartId, [FromBody] List<CartItemDto> cartItems)
        {
            try
            {
                var cartItemsDto = await _cartItemService.AddUpdateItems(cartId, cartItems);
                return cartItemsDto;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpDelete("{cartId}")]
        public async Task<ApiResult<bool>> RemoveItems(Guid cartId, [FromBody] List<Guid> cartItemIds)
        {
            try
            {
                var cartItemsDto = await _cartItemService.RemoveItems(cartId, cartItemIds);
                return cartItemsDto;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }
    }
}
