namespace Mango.Services.OrderAPI.Database.Entities
{
    public class OrderItemOutgoing
    {
        public OrderItemOutgoing()
        {
            ProductName = string.Empty;
        }

        public Guid OrderPublicId { get; set; }

        public Guid ProductPublicId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}