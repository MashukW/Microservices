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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetupEntityBaseFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            SetupEntityBaseFields();
            return base.SaveChanges();
        }

        public void SetupEntityBaseFields()
        {
            var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            addedEntities.ForEach(entityEntry =>
            {
                var publicEntity = entityEntry.Entity as IPublicEntity;
                if (publicEntity != null && publicEntity.PublicId == Guid.Empty)
                    publicEntity.PublicId = Guid.NewGuid();

                var dateCreatedEntity = entityEntry.Entity as IDateCreated;
                if (dateCreatedEntity != null)
                    dateCreatedEntity.DateCreated = DateTime.UtcNow;

                var dateUpdatedEntity = entityEntry.Entity as IDateUpdated;
                if (dateUpdatedEntity != null)
                    dateUpdatedEntity.DateUpdated = DateTime.UtcNow;
            });

            var modifiedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            modifiedEntities.ForEach(entityEntry =>
            {
                var dateUpdatedEntity = entityEntry.Entity as IDateUpdated;
                if (dateUpdatedEntity != null)
                    dateUpdatedEntity.DateUpdated = DateTime.UtcNow;
            });
        }
    }
}