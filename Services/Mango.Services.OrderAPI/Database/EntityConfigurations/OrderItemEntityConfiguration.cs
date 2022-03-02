using Mango.Services.OrderAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mango.Services.OrderAPI.Database.EntityConfigurations
{
    public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable($"{nameof(OrderItem)}s");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.PublicId).IsRequired();

            builder
                .HasOne(p => p.Order)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(p => p.OrderId);
        }
    }
}
