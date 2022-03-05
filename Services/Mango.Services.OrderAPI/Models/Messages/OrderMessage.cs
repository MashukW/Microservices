using Shared.Message.Messages;
using System.Text.Json.Serialization;

namespace Mango.Services.OrderAPI.Models.Messages
{
    public class OrderMessage : BaseMessage
    {
        public OrderMessage()
        {
            CouponCode = string.Empty;
            OrderItems = new List<OrderItemMessage>();

            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;

            CardNumber = string.Empty;
            Cvv = string.Empty;
            ExpityMonthYear = string.Empty;
        }

        public DateTime PickupDateTime { get; set; }

        [JsonPropertyName("cartItems")]
        public List<OrderItemMessage> OrderItems { get; set; }

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

    public class OrderItemMessage
    {
        public OrderItemMessage()
        {
            Product = new OrderProductMessage();
        }

        public Guid PublicId { get; set; }

        public int Count { get; set; }

        public OrderProductMessage Product { get; set; }
    }

    public class OrderProductMessage
    {
        public OrderProductMessage()
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
}
