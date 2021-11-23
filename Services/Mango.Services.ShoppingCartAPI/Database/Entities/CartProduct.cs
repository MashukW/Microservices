using Shared.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ShoppingCartAPI.Database.Entities
{
    public class CartProduct : PublicEntity
    {
        public string? Name { get; set; }

        [Range(1, 1000)]
        public double Price { get; set; }

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public string? ImageUrl { get; set; }

        public virtual List<CartItem>? CartItems { get; set; }
    }
}
