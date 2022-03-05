using AutoMapper;
using Mango.Services.ShoppingCartAPI.Accessors.Interfaces;
using Mango.Services.ShoppingCartAPI.Models.Api;
using Mango.Services.ShoppingCartAPI.Services.Interfaces;
using Shared.Models.Requests;
using Shared.Services.Interfaces;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IApiService _httpService;
        private readonly IMapper _mapper;
        private readonly ITokenAccessor _tokenAccessor;

        public CouponService(IApiService httpService, IMapper mapper, ITokenAccessor tokenAccessor)
        {
            _httpService = httpService;
            _mapper = mapper;
            _tokenAccessor = tokenAccessor;
        }

        public async Task<CouponApi> GetCoupon(string couponCode)
        {
            // var token = await _tokenAccessor.GetAccessToken();
            // var requestDetails = ApiRequest.Create(AppConstants.CouponApi, $"api/coupon/{couponCode}", HttpMethod.Get, token);
            // 
            // var getCouponResult = await _httpService.Send<CouponApi>(requestDetails);
            // // return _mapper.Map<CouponApi>(getCouponResult.Data);

            throw new NotImplementedException();
        }
    }
}