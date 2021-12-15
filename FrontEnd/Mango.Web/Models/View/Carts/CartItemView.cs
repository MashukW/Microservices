namespace Mango.Web.Models.View.Carts
{
    public class CartItemView
    {
        public CartItemView()
        {
            Product = new CartProductView();
        }

        public Guid PublicId { get; set; }

        public int Count { get; set; }

        public CartProductView Product { get; set; }
    }
}
