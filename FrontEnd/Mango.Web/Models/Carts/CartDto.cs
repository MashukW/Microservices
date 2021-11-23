namespace Mango.Web.Models.Carts
{
    public class CartDto
    {
        public Guid PublicId { get; set; }

        public Guid UserPublicId { get; set; }

        public string? CouponCode { get; set; }

        public List<CartItemDto>? CartItems { get; set; }
    }
}
