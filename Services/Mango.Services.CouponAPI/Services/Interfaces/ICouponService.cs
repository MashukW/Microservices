using Mango.Services.CouponAPI.Models.Api;
using Shared.Models.OperationResults;

namespace Mango.Services.CouponAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<Result<CouponApi>> GetByCode(string couponCode);
    }
}
