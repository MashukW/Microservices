namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    public class CartDto
    {
        public Guid PublicId { get; set; }

        public Guid UserPublicId { get; set; }

        public string? CouponCode { get; set; }

        public List<CartItemDto>? CartItems { get; set; }
    }
}
