using AutoMapper;
using Mango.Web.Accessors.Interfaces;
using Mango.Web.Models.Api.Coupons;
using Mango.Web.Models.Service;
using Shared.Models.Requests;
using Shared.Services.Interfaces;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly ITokenAccessor _tokenAccessor;
        private readonly IMapper _mapper;
        private readonly IApiService _httpService;

        public CouponService(IApiService httpService, ITokenAccessor tokenAccessor, IMapper mapper)
        {
            _tokenAccessor = tokenAccessor;
            _mapper = mapper;
            _httpService = httpService;
        }

        public async Task<Coupon> Get(string couponCode)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = RequestData.Create(AppConstants.CouponApi, $"api/coupon/{couponCode}", HttpMethod.Get, token);

            var getCouponResult = await _httpService.Send<CouponApi>(requestDetails);
            return _mapper.Map<Coupon>(getCouponResult.Data);
        }
    }
}
