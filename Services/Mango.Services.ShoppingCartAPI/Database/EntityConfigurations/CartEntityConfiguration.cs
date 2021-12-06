using Mango.Services.ShoppingCartAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mango.Services.ShoppingCartAPI.Database.EntityConfigurations
{
    public class CartEntityConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable($"{nameof(Cart)}s");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.PublicId).IsRequired();
            builder.Property(p => p.UserPublicId).IsRequired();
            builder.Property(p => p.DateCreated).IsRequired();
            builder.Property(p => p.DateUpdated).IsRequired();
        }
    }
}
