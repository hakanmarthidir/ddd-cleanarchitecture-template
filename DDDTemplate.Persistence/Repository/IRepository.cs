using DDDTemplate.Domain.Shared;

namespace DDDTemplate.Persistence.Repository
{
    public interface IRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity> where TEntity : class, IEntity
    {

    }
}
