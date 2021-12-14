using Mango.Web.Models.Coupons;
using Shared.Models.OperationResults;

namespace Mango.Web.Services
{
    public interface ICouponService
    {
        Task<Result<CouponDto>> Get(string couponCode, string? token = null);
    }
}
