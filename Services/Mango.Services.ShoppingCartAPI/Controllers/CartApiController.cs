using Mango.Services.ShoppingCartAPI.Models.Api;
using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;
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
        public async Task<ApiResponse<CartOutgoing>> Get()
        {
            var cart = await _cartService.Get();
            return cart;
        }

        [HttpPost("add-items")]
        public async Task<ApiResponse<CartOutgoing>> AddItems([FromBody] List<CartItemIncoming> cartItemsIncoming)
        {
            var userCart = await _cartService.AddItems(cartItemsIncoming);
            return userCart;
        }

        [HttpPut("update-items")]
        public async Task<ApiResponse<CartOutgoing>> UpdateItems([FromBody] List<CartItemIncoming> cartItemsIncoming)
        {
            var userCart = await _cartService.UpdateItems(cartItemsIncoming);
            return userCart;
        }

        [HttpDelete("remove-items")]
        public async Task<ApiResponse<bool>> RemoveItems([FromBody] List<Guid> cartItemPublicIds)
        {
            var isSuccess = await _cartService.RemoveItems(cartItemPublicIds);
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