using Mango.Services.ShoppingCartAPI.Models.Api;

namespace Mango.Services.ShoppingCartAPI.Models.Messages
{
    public class CheckoutMessage
    {
        public CheckoutMessage()
        {
            CouponCode = string.Empty;
            CartItems = new List<CartItemApi>();

            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;

            CardNumber = string.Empty;
            Cvv = string.Empty;
            ExpityMonthYear = string.Empty;
        }

        public DateTime PickupDateTime { get; set; }


        public List<CartItemApi> CartItems { get; set; }

        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }

        public double TotalCost { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }


        public string CardNumber { get; set; }

        public string Cvv { get; set; }

        public string ExpityMonthYear { get; set; }
    }
}
