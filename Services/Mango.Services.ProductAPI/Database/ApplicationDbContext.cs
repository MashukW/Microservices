using Mango.Services.ProductAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Database;
using System.Reflection;

namespace Mango.Services.ProductAPI.Database
{
    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            SeedFakeData(modelBuilder);
        }

        private static void SeedFakeData(ModelBuilder modelBuilder)
        {
            SeedFakeProducts(modelBuilder);
        }

        private static void SeedFakeProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 1,
                PublicId = Guid.NewGuid(),
                Name = "Samosa",
                Price = 15,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://learnmicroservicessa.blob.core.windows.net/mango/products/14.jpg", 
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 2,
                PublicId = Guid.NewGuid(),
                Name = "Paneer Tikka",
                Price = 13.99,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://learnmicroservicessa.blob.core.windows.net/mango/products/12.jpg",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 3,
                PublicId = Guid.NewGuid(),
                Name = "Sweet Pie",
                Price = 10.99,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://learnmicroservicessa.blob.core.windows.net/mango/products/11.jpg",
                CategoryName = "Dessert"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 4,
                PublicId = Guid.NewGuid(),
                Name = "Pav Bhaji",
                Price = 15,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://learnmicroservicessa.blob.core.windows.net/mango/products/13.jpg",
                CategoryName = "Entree"
            });
        }
    }
}
