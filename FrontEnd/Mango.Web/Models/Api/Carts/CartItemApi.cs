namespace Mango.Web.Models.Api.Carts
{
    public class CartItemApi
    {
        public CartItemApi()
        {
            Product = new CartProductApi();
        }

        public Guid PublicId { get; set; }

        public int Count { get; set; }

        public CartProductApi Product { get; set; }
    }
}
