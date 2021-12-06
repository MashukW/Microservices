using AutoMapper;
using Mango.Services.CouponAPI.Entities;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;
using Shared.Models.OperationResults;

namespace Mango.Services.CouponAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IRepository<Coupon> _couponRepository;
        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public CouponService(IRepository<Coupon> couponRepository, IWorkUnit workUnit, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _workUnit = workUnit;
            _mapper = mapper;
        }

        public async Task<Result<CouponDto>> GetByCode(string couponCode)
        {
            var coupon = await _couponRepository.Query().FirstOrDefaultAsync(x => x.Code == couponCode);
            return _mapper.Map<CouponDto>(coupon);
        }
    }
}
