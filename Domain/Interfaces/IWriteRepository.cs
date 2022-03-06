using Domain.Shared;

namespace Domain.Interfaces
{
    public interface IWriteRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
    {
        void Insert(TEntity entity);
        void InsertAll(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete<TId>(TId id);
        Task InsertAsync(TEntity entity, CancellationToken token = default(CancellationToken));
        Task InsertAllAsync(IEnumerable<TEntity> entities, CancellationToken token = default(CancellationToken));
        Task UpdateAsync(TEntity entity, CancellationToken token = default(CancellationToken));
        Task DeleteAsync<TId>(TId id, CancellationToken token = default(CancellationToken));
    }
}
