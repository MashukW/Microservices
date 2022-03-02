using Shared.Database.Entities;

namespace Mango.Services.OrderAPI.Database.Entities
{
    public class Order : DateTrackedPublicEntity
    {
        public Order()
        {
            CouponCode = string.Empty;
            OrderItems = new List<OrderItem>();

            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;

            CardNumber = string.Empty;
            Cvv = string.Empty;
            ExpityMonthYear = string.Empty;
        }

        public bool PaymentStatus { get; set; }

        public DateTime OrderDataTime { get; set; }

        public DateTime PickupDateTime { get; set; }

        public List<OrderItem> OrderItems { get; set; }

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
}
