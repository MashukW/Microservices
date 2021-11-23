using Shared.Database.Entities;

namespace Mango.Services.ProductAPI.Database.Entities
{
    public class Product : PublicEntity
    {
        public string? Name { get; set; }

        public double Price { get; set; }

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
