namespace Mango.Services.ShoppingCartAPI.Models.Api
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
