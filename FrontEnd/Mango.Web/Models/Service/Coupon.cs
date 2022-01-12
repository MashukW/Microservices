namespace Mango.Web.Models.Service
{
    public class Coupon
    {
        public Guid PublicId { get; set; }

        public string? Code { get; set; }

        public double DiscountAmount { get; set; }
    }
}
