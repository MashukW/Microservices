using Shared.Database.Entities;

namespace Mango.Services.OrderAPI.Database.Entities
{
    public class OrderItem : DateTrackedPublicEntity
    {
        public OrderItem()
        {
            ProductName = string.Empty;
            Order = new Order();
        }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductPublicId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}