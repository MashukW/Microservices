using Mango.Web.Accessors.Interfaces;
using Mango.Web.Models.Api.Coupons;
using Shared.Models.OperationResults;
using Shared.Models.Requests;
using Shared.Services.Interfaces;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly ITokenAccessor _tokenAccessor;
        private readonly IHttpService _httpService;

        public CouponService(IHttpService httpService, ITokenAccessor tokenAccessor)
        {
            _tokenAccessor = tokenAccessor;
            _httpService = httpService;
        }

        public async Task<Result<CouponApi>> Get(string couponCode)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = RequestData.Create(AppConstants.CouponApi, $"api/coupon/{couponCode}", HttpMethod.Get, token);

            var getCouponResult = await _httpService.Send<CouponApi>(requestDetails);
            return getCouponResult;
        }
    }
}
