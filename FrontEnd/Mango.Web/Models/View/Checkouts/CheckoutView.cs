using Mango.Web.Models.View.Carts;

namespace Mango.Web.Models.View.Checkouts
{
    public class CheckoutView
    {
        public CheckoutView()
        {
            CouponCode = string.Empty;
            CartItems = new List<CartItemView>();
            UserDetails = new CheckoutUserDetailsView();
            PaymentCard = new CheckoutUserCardView();
        }

        public DateTime PickupDateTime { get; set; }

        public string CouponCode { get; set; }

        public List<CartItemView> CartItems { get; set; }

        public double DiscountAmount { get; set; }

        public double TotalCost {get;set; }

        public CheckoutUserDetailsView UserDetails { get; set; }

        public CheckoutUserCardView PaymentCard { get; set; }
    }
}
