namespace Mango.Web.Models.Api.Carts
{
    public class CartApi
    {

        public Guid PublicId { get; set; }

        public Guid UserPublicId { get; set; }

        public string? CouponCode { get; set; }

        public List<CartItemApi>? CartItems { get; set; }
    }
}
