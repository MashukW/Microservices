using Mango.Services.ShoppingCartAPI.Models.Api;

namespace Mango.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponApi> GetCoupon(string couponCode);
    }
}
