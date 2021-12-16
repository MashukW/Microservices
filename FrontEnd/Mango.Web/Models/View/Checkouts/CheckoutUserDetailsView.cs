using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.View.Checkouts
{
    public class CheckoutUserDetailsView
    {
        public CheckoutUserDetailsView()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
