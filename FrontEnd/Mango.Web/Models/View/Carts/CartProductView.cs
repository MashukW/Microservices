using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.View.Carts
{
    public class CartProductView
    {
        public CartProductView()
        {
            Name = string.Empty;
            Description = string.Empty;
            CategoryName = string.Empty;
            ImageUrl = string.Empty;
        }

        public Guid PublicId { get; set; }

        public string Name { get; set; }

        [Range(1, 1000)]
        public double Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }
    }
}
