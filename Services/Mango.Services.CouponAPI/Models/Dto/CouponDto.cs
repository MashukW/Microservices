namespace Mango.Services.CouponAPI.Models
{
    public class CouponDto
    {
        public Guid PublicId { get; set; }

        public string? Code { get; set; }

        public double DiscountAmount { get; set; }
    }
}
