using DDDTemplate.Domain.Shared;

namespace DDDTemplate.Domain.Interfaces
{
    public interface IRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
    {

    }
}
