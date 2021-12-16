using Shared.Database.Entities;

namespace Mango.Services.ShoppingCartAPI.Database.Entities
{
    public class CartItem : DateTrackedPublicEntity
    {
        public CartItem()
        {
            Cart = new Cart();
            Product = new CartProduct();
        }

        public int Count { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public int ProductId { get; set; }
        public CartProduct Product { get; set; }
    }
}
