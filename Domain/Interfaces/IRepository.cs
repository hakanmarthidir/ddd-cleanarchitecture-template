using Domain.Shared;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity, TKey> : IReadRepository<TEntity, TKey>, IWriteRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>, IAggregateRoot
    {

    }
}
