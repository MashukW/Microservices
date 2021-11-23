namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    public class CartItemDto
    {
        public Guid PublicId { get; set; }

        public int Count { get; set; }

        public CartProductDto Product { get; set; }
    }
}
