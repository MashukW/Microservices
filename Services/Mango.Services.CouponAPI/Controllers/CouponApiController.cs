using Mango.Services.CouponAPI.Models.Api;
using Mango.Services.CouponAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.ApiResponses;

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
        public async Task<ApiResponse<CouponApi>> Get(string couponCode)
        {
            var coupon = await _couponService.GetByCode(couponCode);
            return coupon;
        }
    }
}