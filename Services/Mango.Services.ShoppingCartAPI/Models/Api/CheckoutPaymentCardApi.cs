namespace Mango.Services.ShoppingCartAPI.Models.Api
{
    public class CheckoutPaymentCardApi
    {
        public CheckoutPaymentCardApi()
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
