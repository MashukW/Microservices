using Mango.Web.Models.Coupons;
using Shared.Models.OperationResults;
using Shared.Models.Requests;
using Shared.Services;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpService _httpService;

        public CouponService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<Result<CouponDto>> Get(string couponCode, string? token = null)
        {
            var requestDetails = RequestData.Create(AppConstants.CouponApi, $"api/coupon/{couponCode}", HttpMethod.Get, token);

            var getCouponResponse = await _httpService.Send<CouponDto>(requestDetails);
            return getCouponResponse;
        }
    }
}
