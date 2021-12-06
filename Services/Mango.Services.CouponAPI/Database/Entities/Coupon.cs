using Shared.Database.Entities;

namespace Mango.Services.CouponAPI.Entities
{
    public class Coupon : PublicEntity
    {
        public string? Code { get; set; }

        public double DiscountAmount { get; set; }
    }
}
