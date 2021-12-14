namespace Mango.Web.Models.Carts
{
    public class CartDto
    {
        public Guid PublicId { get; set; }

        public Guid UserPublicId { get; set; }

        public string? CouponCode { get; set; }

        public double? DiscountAmount { get; set; }

        public List<CartItemDto>? CartItems { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime PickupDateTime { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? CardNumber { get; set; }

        public string? Cvv { get; set; }

        public string? ExpityMonthYear { get; set; }

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

            if (!string.IsNullOrWhiteSpace(CouponCode) && DiscountAmount != null)
            {
                totalCost -= DiscountAmount.Value;
            }

            return totalCost;
        }
    }
}
