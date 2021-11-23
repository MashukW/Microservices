using Microsoft.EntityFrameworkCore;
using Shared.Database.Entities;
using System.Linq.Expressions;

namespace Shared.Database.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, IPublicEntity
    {
        private readonly BaseDbContext _baseContext;

        public Repository(BaseDbContext baseContext)
        {
            _baseContext = baseContext;
        }

        public IQueryable<T> Query()
        {
            return _baseContext.Set<T>().AsQueryable<T>();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return _baseContext.Set<T>().AsQueryable<T>().Where(predicate);
        }

        public async Task<List<T>> Get()
        {
            var tEntities = await Query().AsNoTracking().ToListAsync();
            return tEntities;
        }

        public async Task<List<T>?> Get(Expression<Func<T, bool>> predicate)
        {
            var tEntities = await Query().AsNoTracking().Where(predicate).ToListAsync();
            return tEntities;
        }

        public async Task<T> Add(T entity)
        {
            var tSet = _baseContext.Set<T>();

            var addedEntity = await tSet.AddAsync(entity);
            return addedEntity.Entity;
        }

        public async Task<T> Update(T entity)
        {
            var tSet = _baseContext.Set<T>();

            var updatedEntity = await Task.Run(() => tSet.Update(entity));
            return updatedEntity.Entity;
        }

        public async Task<bool> Remove(T entity)
        {
            var tSet = _baseContext.Set<T>();

            await Task.Run(() => tSet.Remove(entity));
            return true;
        }
    }
}
