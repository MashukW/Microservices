namespace Mango.Services.ShoppingCartAPI.Models.Entities
{
    public class CartItem
    {
        public CartItem()
        {
            Product = new CartProduct();
        }

        public Guid PublicId { get; set; }

        public int Count { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public CartProduct Product { get; set; }
    }
}
