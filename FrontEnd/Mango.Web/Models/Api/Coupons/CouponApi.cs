namespace Mango.Web.Models.Api.Coupons
{
    public class CouponApi
    {
        public Guid PublicId { get; set; }

        public string? Code { get; set; }

        public double DiscountAmount { get; set; }
    }
}
