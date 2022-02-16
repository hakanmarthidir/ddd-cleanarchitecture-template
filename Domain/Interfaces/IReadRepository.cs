using Domain.Shared;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IReadRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
    {
        TEntity FindById(Guid id);
        Task<TEntity> FindByIdAsync(Guid id, CancellationToken token = default(CancellationToken));

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int? page = null, int? pageSize = null);

        Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int? page = null, int? pageSize = null);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    }
}
