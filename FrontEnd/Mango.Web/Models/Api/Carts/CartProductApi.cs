﻿namespace Mango.Web.Models.Api.Carts
{
    public class CartProductApi 
    {
        public CartProductApi()
        {
            Name = string.Empty;
            Description = string.Empty;
            CategoryName = string.Empty;
            ImageUrl = string.Empty;
        }

        public Guid PublicId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }
    }
}
