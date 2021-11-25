using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Responses;

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
        public async Task<ResponseData<CartDto>> Get(Guid userId)
        {
            try
            {
                var cartDto = await _cartService.Get(userId);
                return cartDto;
            }
            catch (Exception ex)
            {
                return ResponseData<CartDto>.Fail(ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ResponseData<CartDto>> AddCart([FromBody] CartDto cartDto)
        {
            try
            {
                var newCart = await _cartService.Create(cartDto);
                return newCart;
            }
            catch (Exception ex)
            {
                return ResponseData<CartDto>.Fail(ex.Message);
            }
        }

        [HttpDelete("clear/{userId}")]
        public async Task<ResponseData<bool>> ClearCart(Guid cartId)
        {
            try
            {
                var isSuccess = await _cartService.Clear(cartId);
                return isSuccess;
            }
            catch (Exception ex)
            {
                return ResponseData<bool>.Fail(ex.Message);
            }
        }
    }
}