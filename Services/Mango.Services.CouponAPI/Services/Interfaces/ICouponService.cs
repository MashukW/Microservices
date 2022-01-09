using Mango.Services.CouponAPI.Models.Api;

namespace Mango.Services.CouponAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponApi> GetByCode(string couponCode);
    }
}
