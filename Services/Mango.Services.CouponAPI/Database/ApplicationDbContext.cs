using Mango.Services.CouponAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Database;
using System.Reflection;

namespace Mango.Services.CouponAPI.Database
{
    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            SeedFakeData(modelBuilder);
        }

        private static void SeedFakeData(ModelBuilder modelBuilder)
        {
            SeedFakeCoupons(modelBuilder);
        }

        private static void SeedFakeCoupons(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 1,
                PublicId = Guid.NewGuid(),
                Code = "10OFF",
                DiscountAmount = 10
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 2,
                PublicId = Guid.NewGuid(),
                Code = "20OFF",
                DiscountAmount = 20
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 3,
                PublicId = Guid.NewGuid(),
                Code = "30OFF",
                DiscountAmount = 30
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 4,
                PublicId = Guid.NewGuid(),
                Code = "40OFF",
                DiscountAmount = 40
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 5,
                PublicId = Guid.NewGuid(),
                Code = "50OFF",
                DiscountAmount = 50
            });
        }
    }
}
