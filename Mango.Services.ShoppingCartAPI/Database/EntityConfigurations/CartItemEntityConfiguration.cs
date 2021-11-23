using Mango.Services.ShoppingCartAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mango.Services.ShoppingCartAPI.Database.EntityConfigurations
{
    public class CartItemEntityConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PublicId).IsRequired();

            builder.HasOne(p => p.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(p => p.ProductId);

            builder
                .HasOne(p => p.Cart)
                .WithMany(p => p.CartItems)
                .HasForeignKey(p => p.CartId);
        }
    }
}
