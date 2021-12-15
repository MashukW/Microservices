namespace Mango.Web.Models.View.Carts
{
    public class CartView
    {
        public CartView()
        {
            CouponCode = string.Empty;
            CartItems = new List<CartItemView>();
        }

        public Guid PublicId { get; set; }

        public Guid UserPublicId { get; set; }

        public string CouponCode { get; set; }

        public List<CartItemView> CartItems { get; set; }

        public double DiscountAmount { get; set; }

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

            if (!string.IsNullOrWhiteSpace(CouponCode) && DiscountAmount > 0)
            {
                totalCost -= DiscountAmount;
            }

            return totalCost;
        }
    }
}
