namespace Mango.Services.ShoppingCartAPI.Models.Outgoing
{
    public class CartItemOutgoing
    {
        public CartItemOutgoing()
        {
            Product = new CartProductOutgoing();
        }

        public Guid PublicId { get; set; }

        public int Count { get; set; }

        public CartProductOutgoing Product { get; set; }
    }
}
