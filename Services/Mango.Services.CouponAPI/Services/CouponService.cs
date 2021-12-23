using AutoMapper;
using Mango.Services.CouponAPI.Entities;
using Mango.Services.CouponAPI.Models.Api;
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

        public async Task<Result<CouponApi>> GetByCode(string couponCode)
        {
            // This is code from branch test 1
            // This is code from branch test 2
            // This is code from branch test 3
            // This is code from branch test 4
            // This is code from branch test 5

            var coupon = await _couponRepository.Query().FirstOrDefaultAsync(x => x.Code == couponCode);
            return _mapper.Map<CouponApi>(coupon);
        }
    }
}
