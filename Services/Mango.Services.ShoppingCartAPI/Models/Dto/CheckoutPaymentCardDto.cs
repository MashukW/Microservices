namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    public class CheckoutPaymentCardDto
    {
        public string? CardNumber { get; set; }

        public string? Cvv { get; set; }

        public string? ExpityMonthYear { get; set; }
    }
}
