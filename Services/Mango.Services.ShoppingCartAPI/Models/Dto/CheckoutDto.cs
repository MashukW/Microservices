namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    public class CheckoutDto
    {
        public DateTime PickupDateTime { get; set; }

        public CartDto? Cart { get; set; }

        public CheckoutUserDetailsDto? UserDetails { get; set; }

        public CheckoutPaymentCardDto? PaymentCard { get; set; }
    }
}
