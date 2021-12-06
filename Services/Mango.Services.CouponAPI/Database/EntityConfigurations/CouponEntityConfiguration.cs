using Mango.Services.CouponAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mango.Services.CouponAPI.Database.EntityConfigurations
{
    public class CouponEntityConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable($"{nameof(Coupon)}s");

            builder.Property(s => s.Code).IsRequired();
            builder.Property(s => s.DiscountAmount).IsRequired();
        }
    }
}
