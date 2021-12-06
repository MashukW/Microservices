namespace Mango.Web.Models.Carts
{
    public class CartDto
    {
        public Guid PublicId { get; set; }

        public Guid UserPublicId { get; set; }

        public string? CouponCode { get; set; }

        public List<CartItemDto>? CartItems { get; set; }

        public double TotalCost => CalculateTotalCost();

        private double CalculateTotalCost()
        {
            var totalCost = 0d;

            if (CartItems != null)
            {
                foreach (var cartItem in CartItems)
                {
                    totalCost += cartItem.Count * cartItem.Product.Price;
                }
            }

            return totalCost;
        }
    }
}
