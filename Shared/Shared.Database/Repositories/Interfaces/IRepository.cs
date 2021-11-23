using Shared.Database.Entities;
using System.Linq.Expressions;

namespace Shared.Database.Repositories
{
    public interface IRepository<T> where T : class, IEntity, IPublicEntity
    {
        IQueryable<T> Query();

        IQueryable<T> Query(Expression<Func<T, bool>> predicate);

        Task<List<T>> Get();

        Task<List<T>?> Get(Expression<Func<T, bool>> predicate);

        Task<T> Add(T entity);

        Task<T> Update(T entity);

        Task<bool> Remove(T entity);
    }
}
