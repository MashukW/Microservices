using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;
using Shared.Models.OperationResults;

namespace Mango.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponApiController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponApiController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet("{couponCode}")]
        public async Task<ApiResult<CouponDto>> Get(string couponCode)
        {
            try
            {
                var coupon = await _couponService.GetByCode(couponCode);
                return coupon;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }
    }
}