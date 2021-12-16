namespace Mango.Services.ShoppingCartAPI.Models.Api
{
    public class CartApi
    {
        public CartApi()
        {
            CouponCode = string.Empty;
            CartItems = new List<CartItemApi>();
        }

        public Guid PublicId { get; set; }

        public Guid UserPublicId { get; set; }

        public string CouponCode { get; set; }

        public List<CartItemApi> CartItems { get; set; }
    }
}
