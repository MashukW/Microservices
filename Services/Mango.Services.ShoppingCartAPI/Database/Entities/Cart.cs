using Shared.Database.Entities;

namespace Mango.Services.ShoppingCartAPI.Database.Entities
{
    public class Cart : DateTrackedPublicEntity
    {
        public Cart()
        {
            CouponCode = string.Empty;
            CartItems = new List<CartItem>();
        }

        public Guid UserPublicId { get; set; }

        public string CouponCode { get; set; }

        public IList<CartItem> CartItems { get; set; }
    }
}
