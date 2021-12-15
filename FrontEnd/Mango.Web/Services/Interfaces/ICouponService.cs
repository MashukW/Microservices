using Mango.Web.Models.Api.Coupons;
using Shared.Models.OperationResults;

namespace Mango.Web.Services
{
    public interface ICouponService
    {
        Task<Result<CouponApi>> Get(string couponCode);
    }
}
