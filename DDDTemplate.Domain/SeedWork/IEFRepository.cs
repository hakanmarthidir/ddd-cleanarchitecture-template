namespace DDDTemplate.Domain.SeedWork
{
    public interface IEFRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity>, IWriteAsyncRepository<TEntity> where TEntity : class, IEntity
    {

    }
}
