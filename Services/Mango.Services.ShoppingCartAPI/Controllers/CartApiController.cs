using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;
using Mango.Services.ShoppingCartAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.ApiResponses;

namespace Mango.Services.ShoppingCartAPI.Controllers;

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
    public ApiResponse<CartOutgoing> Get()
    {
        var cart = _cartService.Get();
        return cart;
    }

    [HttpPost("add-item")]
    public ApiResponse<CartOutgoing> AddItems([FromBody] CartItemIncoming cartItemIncoming)
    {
        var userCart = _cartService.AddItem(cartItemIncoming);
        return userCart;
    }

    [HttpDelete("remove-item")]
    public ApiResponse<bool> RemoveItems([FromBody] Guid cartItemPublicId)
    {
        var isSuccess = _cartService.RemoveItem(cartItemPublicId);
        return isSuccess;
    }

    [HttpPost("apply-coupon")]
    public ApiResponse<bool> ApplyCoupon([FromBody] string couponCode)
    {
        var isSuccess = _cartService.ApplyCoupon(couponCode);
        return isSuccess;
    }

    [HttpDelete("remove-coupon")]
    public ApiResponse<bool> RemoveCoupon()
    {
        var isSuccess = _cartService.RemoveCoupon();
        return isSuccess;
    }

    [HttpPost("checkout")]
    public async Task<ApiResponse<bool>> Checkout([FromBody] CheckoutIncoming checkoutIncoming)
    {
        var isSuccess = await _cartService.Checkout(checkoutIncoming);
        return isSuccess;
    }

    [HttpDelete("clear")]
    public ApiResponse<bool> Clear()
    {
        var isSuccess = _cartService.Clear();
        return isSuccess;
    }
}