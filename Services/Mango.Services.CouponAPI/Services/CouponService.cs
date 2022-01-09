using AutoMapper;
using Mango.Services.CouponAPI.Entities;
using Mango.Services.CouponAPI.Models.Api;
using Mango.Services.CouponAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;

namespace Mango.Services.CouponAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IRepository<Coupon> _couponRepository;
        private readonly IMapper _mapper;

        public CouponService(IRepository<Coupon> couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        public async Task<CouponApi> GetByCode(string couponCode)
        {
            var coupon = await _couponRepository.Query().FirstOrDefaultAsync(x => x.Code == couponCode);
            return _mapper.Map<CouponApi>(coupon);
        }
    }
}
