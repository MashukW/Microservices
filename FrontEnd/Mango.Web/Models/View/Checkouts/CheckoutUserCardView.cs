using System.ComponentModel.DataAnnotations;

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

        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage ="Card length must be 16 sybmols")]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVV length must be 3 sybmols")]
        public string Cvv { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Expity month year must be in the next format: MMYY")]
        public string ExpityMonthYear { get; set; }
    }
}
