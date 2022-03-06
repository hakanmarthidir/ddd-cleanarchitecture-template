using Domain.Shared;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IReadRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
    {
        TEntity FindById<TId>(TId id);
        Task<TEntity> FindByIdAsync<TId>(TId id, CancellationToken token = default(CancellationToken));

        ICollection<TEntity> Find(ISpec<TEntity> spec);

        Task<ICollection<TEntity>> FindAsync(ISpec<TEntity> spec, CancellationToken token = default(CancellationToken));

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken));
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    }
}
