namespace Mango.Services.ShoppingCartAPI.Models.Entities
{
    public class Cart
    {
        public Cart()
        {
            CouponCode = string.Empty;
            CartItems = new List<CartItem>();
        }

        public Guid PublicId { get; set; }

        public Guid UserPublicId { get; set; }

        public string CouponCode { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public IList<CartItem> CartItems { get; set; }

        public bool IsInitial()
        {
            return PublicId == default;
        }
    }
}
