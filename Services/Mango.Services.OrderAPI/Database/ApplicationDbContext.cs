using Microsoft.EntityFrameworkCore;
using Shared.Database;
using System.Reflection;

namespace Mango.Services.OrderAPI.Database
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
        }
    }
}
