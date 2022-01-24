using Shared.Message.Messages;

namespace Mango.Services.ShoppingCartAPI.Models.Messages
{
    public class CheckoutMessage : BaseMessage
    {
        public CheckoutMessage()
        {
            CouponCode = string.Empty;
            CartItems = new List<CartItemMessage>();

            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;

            CardNumber = string.Empty;
            Cvv = string.Empty;
            ExpityMonthYear = string.Empty;
        }

        public DateTime PickupDateTime { get; set; }

        public List<CartItemMessage> CartItems { get; set; }

        public int TotalItems { get; set; }

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

    public class CartProductMessage
    {
        public CartProductMessage()
        {
            Name = string.Empty;
            Description = string.Empty;
            CategoryName = string.Empty;
            ImageUrl = string.Empty;
        }

        public Guid PublicId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }
    }

    public class CartItemMessage
    {
        public CartItemMessage()
        {
            Product = new CartProductMessage();
        }

        public Guid PublicId { get; set; }

        public int Count { get; set; }

        public CartProductMessage Product { get; set; }
    }
}
