using Microsoft.EntityFrameworkCore;
using Shared.Database.Entities;

namespace Shared.Database
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public override int SaveChanges()
        {
            var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            addedEntities.ForEach(entityEntry =>
            {
                if (entityEntry is IPublicEntity publicEntity && publicEntity.PublicId == Guid.Empty)
                    publicEntity.PublicId = Guid.NewGuid();

                if (entityEntry is IDateCreated dateCreatedEntity)
                    dateCreatedEntity.DateCreated = DateTime.UtcNow;

                if (entityEntry is IDateUpdated dateUpdatedEntity)
                    dateUpdatedEntity.DateUpdated = DateTime.UtcNow;
            });

            var modifiedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            modifiedEntities.ForEach(entityEntry =>
            {
                if (entityEntry is IDateUpdated dateUpdatedEntity)
                    dateUpdatedEntity.DateUpdated = DateTime.UtcNow;
            });

            return base.SaveChanges();
        }
    }
}