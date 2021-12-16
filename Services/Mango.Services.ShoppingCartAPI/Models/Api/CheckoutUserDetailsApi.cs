namespace Mango.Services.ShoppingCartAPI.Models.Api
{
    public class CheckoutUserDetailsApi
    {
        public CheckoutUserDetailsApi()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
