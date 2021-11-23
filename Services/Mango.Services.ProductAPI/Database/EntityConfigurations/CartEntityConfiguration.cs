using Mango.Services.ProductAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mango.Services.ProductAPI.Database.EntityConfigurations
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(s => s.Name).IsRequired();
            builder.Property(s => s.Price).IsRequired();
        }
    }
}
