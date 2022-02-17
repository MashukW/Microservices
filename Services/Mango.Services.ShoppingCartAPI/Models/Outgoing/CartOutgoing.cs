namespace Mango.Services.ShoppingCartAPI.Models.Outgoing
{
    public class CartOutgoing
    {
        public CartOutgoing()
        {
            CouponCode = string.Empty;
            CartItems = new List<CartItemOutgoing>();
        }

        public Guid PublicId { get; set; }

        public Guid UserPublicId { get; set; }

        public string CouponCode { get; set; }

        public List<CartItemOutgoing> CartItems { get; set; }
    }
}
