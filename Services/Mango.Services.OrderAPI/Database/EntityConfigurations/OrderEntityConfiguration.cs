using Mango.Services.OrderAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mango.Services.OrderAPI.Database.EntityConfigurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable($"{nameof(Order)}s");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.PublicId).IsRequired();
            builder.Property(p => p.DateCreated).IsRequired();
            builder.Property(p => p.DateUpdated).IsRequired();
        }
    }
}
