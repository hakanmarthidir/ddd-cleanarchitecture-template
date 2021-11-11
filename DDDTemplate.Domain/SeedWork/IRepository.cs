namespace DDDTemplate.Domain.SeedWork
{
    public interface IRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity> where TEntity : class, IEntity
    {

    }
}
