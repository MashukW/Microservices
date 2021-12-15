namespace Mango.Web.Models.View.Checkouts
{
    public class CheckoutUserCardView
    {
        public CheckoutUserCardView()
        {
            CardNumber = string.Empty;
            Cvv = string.Empty;
            ExpityMonthYear = string.Empty;
        }

        public string CardNumber { get; set; }

        public string Cvv { get; set; }

        public string ExpityMonthYear { get; set; }
    }
}
