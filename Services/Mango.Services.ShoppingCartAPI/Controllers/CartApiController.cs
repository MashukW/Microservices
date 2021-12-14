using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;
using Shared.Models.OperationResults;

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
        public async Task<ApiResult<CartDto>> Get()
        {
            try
            {
                var cartDto = await _cartService.Get();
                return cartDto;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpPost("add-items")]
        public async Task<ApiResult<CartDto>> AddItems([FromBody] List<CartItemDto> cartItemsDto)
        {
            try
            {
                var userCart = await _cartService.AddItems(cartItemsDto);
                return userCart;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpPut("update-items")]
        public async Task<ApiResult<CartDto>> UpdateItems([FromBody] List<CartItemDto> cartItemsDto)
        {
            try
            {
                var userCart = await _cartService.UpdateItems(cartItemsDto);
                return userCart;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpDelete("remove-items")]
        public async Task<ApiResult<bool>> RemoveItems([FromBody] List<Guid> cartItemsPublicIds)
        {
            try
            {
                var isSuccess = await _cartService.RemoveItems(cartItemsPublicIds);
                return isSuccess;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpPost("apply-coupon")]
        public async Task<ApiResult<bool>> ApplyCoupon([FromBody] string couponCode)
        {
            try
            {
                var isSuccess = await _cartService.ApplyCoupon(couponCode);
                return isSuccess;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpDelete("remove-coupon")]
        public async Task<ApiResult<bool>> RemoveCoupon()
        {
            try
            {
                var isSuccess = await _cartService.RemoveCoupon();
                return isSuccess;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpDelete("clear")]
        public async Task<ApiResult<bool>> Clear()
        {
            try
            {
                var isSuccess = await _cartService.Clear();
                return isSuccess;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }
    }
}