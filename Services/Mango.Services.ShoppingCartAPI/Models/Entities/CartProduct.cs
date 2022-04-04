using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ShoppingCartAPI.Models.Entities
{
    public class CartProduct
    {
        public Guid PublicId { get; set; }

        public string? Name { get; set; }

        [Range(1, 1000)]
        public double Price { get; set; }

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
