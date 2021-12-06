using Mango.Services.CouponAPI.Models;
using Shared.Models.OperationResults;

namespace Mango.Services.CouponAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<Result<CouponDto>> GetByCode(string couponCode);
    }
}
