using AutoMapper;
using Mango.Services.CouponAPI.Entities;
using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI.MapConfig
{
    public class ApplicationMapConfig : Profile
    {
        public ApplicationMapConfig()
        {
            CreateMap<CouponDto, Coupon>().ReverseMap();
        }
    }
}
