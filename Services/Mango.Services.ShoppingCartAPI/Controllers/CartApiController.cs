using Mango.Services.ShoppingCartAPI.Models.Api;
using Mango.Services.ShoppingCartAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.ApiResponses;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartApiController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartApiController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ApiResponse<CartApi>> Get()
        {
            var cart = await _cartService.Get();
            return cart;
        }

        [HttpPost("add-items")]
        public async Task<ApiResponse<CartApi>> AddItems([FromBody] List<CartItemApi> cartItemsDto)
        {
            var userCart = await _cartService.AddItems(cartItemsDto);
            return userCart;
        }

        [HttpPut("update-items")]
        public async Task<ApiResponse<CartApi>> UpdateItems([FromBody] List<CartItemApi> cartItemsDto)
        {
            var userCart = await _cartService.UpdateItems(cartItemsDto);
            return userCart;
        }

        [HttpDelete("remove-items")]
        public async Task<ApiResponse<bool>> RemoveItems([FromBody] List<Guid> cartItemsPublicIds)
        {
            var isSuccess = await _cartService.RemoveItems(cartItemsPublicIds);
            return isSuccess;
        }

        [HttpPost("apply-coupon")]
        public async Task<ApiResponse<bool>> ApplyCoupon([FromBody] string couponCode)
        {
            var isSuccess = await _cartService.ApplyCoupon(couponCode);
            return isSuccess;
        }

        [HttpDelete("remove-coupon")]
        public async Task<ApiResponse<bool>> RemoveCoupon()
        {
            var isSuccess = await _cartService.RemoveCoupon();
            return isSuccess;
        }

        [HttpPost("checkout")]
        public async Task<ApiResponse<bool>> Checkout([FromBody] CheckoutApi model)
        {
            var isSuccess = await _cartService.Checkout(model);
            return isSuccess;
        }

        [HttpDelete("clear")]
        public async Task<ApiResponse<bool>> Clear()
        {
            var isSuccess = await _cartService.Clear();
            return isSuccess;
        }
    }
}