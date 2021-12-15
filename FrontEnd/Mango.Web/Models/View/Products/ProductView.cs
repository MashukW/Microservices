using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.View.Products
{
    public class ProductView
    {
        public ProductView()
        {
            Name = string.Empty;
            Description = string.Empty;
            CategoryName = string.Empty;
            ImageUrl = string.Empty;
            Count = 1;
        }

        public Guid PublicId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }

        [Range(1, 100)]
        public int Count { get; set; }
    }
}
