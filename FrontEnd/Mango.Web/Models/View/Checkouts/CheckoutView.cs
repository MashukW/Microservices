using Mango.Web.Models.View.Carts;

namespace Mango.Web.Models.View.Checkouts
{
    public class CheckoutView
    {
        public CheckoutView()
        {
            Cart = new CartView();
            UserDetails = new CheckoutUserDetailsView();
            PaymentCard = new CheckoutUserCardView();
        }

        public DateTime PickupDateTime { get; set; }

        public CartView Cart { get; set; }

        public CheckoutUserDetailsView UserDetails { get; set; }

        public CheckoutUserCardView PaymentCard { get; set; }
    }
}
