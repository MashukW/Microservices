using Mango.Web.Models.Service;

namespace Mango.Web.Services
{
    public interface ICouponService
    {
        Task<Coupon> Get(string couponCode);
    }
}
