namespace Mango.Services.ShoppingCartAPI.Models.Incoming
{
    public class CartItemIncoming
    {
        public CartItemIncoming()
        {
            Product = new CartProductIncoming();
        }

        public Guid PublicId { get; set; }

        public int Count { get; set; }

        public CartProductIncoming Product { get; set; }
    }
}
