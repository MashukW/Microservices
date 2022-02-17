using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;
using Mango.Services.ShoppingCartAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.ApiResponses;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Authorize]
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
        public async Task<ApiResponse<CartOutgoing>> Get()
        {
            var cart = await _cartService.Get();
            return cart;
        }

        [HttpPost("add-item")]
        public async Task<ApiResponse<CartOutgoing>> AddItems([FromBody] CartItemIncoming cartItemIncoming)
        {
            var userCart = await _cartService.AddItem(cartItemIncoming);
            return userCart;
        }

        [HttpDelete("remove-item")]
        public async Task<ApiResponse<bool>> RemoveItems([FromBody] Guid cartItemPublicId)
        {
            var isSuccess = await _cartService.RemoveItem(cartItemPublicId);
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
        public async Task<ApiResponse<bool>> Checkout([FromBody] CheckoutIncoming checkoutIncoming)
        {
            var isSuccess = await _cartService.Checkout(checkoutIncoming);
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