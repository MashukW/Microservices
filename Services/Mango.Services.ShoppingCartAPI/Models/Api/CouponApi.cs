namespace Mango.Services.ShoppingCartAPI.Models.Api
{
    public class CouponApi
    {
        public Guid PublicId { get; set; }

        public string? Code { get; set; }

        public double DiscountAmount { get; set; }
    }
}
