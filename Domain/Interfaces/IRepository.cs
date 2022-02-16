using Domain.Shared;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
    {

    }
}
