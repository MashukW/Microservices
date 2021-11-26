using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;
using Shared.Models.OperationResults;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<ApiResult<CartDto>> Get(Guid userId)
        {
            try
            {
                var cartDto = await _cartService.Get(userId);
                return cartDto;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ApiResult<CartDto>> AddCart([FromBody] CartDto cartDto)
        {
            try
            {
                var newCart = await _cartService.Create(cartDto);
                return newCart;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpDelete("clear/{userId}")]
        public async Task<ApiResult<bool>> ClearCart(Guid cartId)
        {
            try
            {
                var isSuccess = await _cartService.Clear(cartId);
                return isSuccess;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }
    }
}