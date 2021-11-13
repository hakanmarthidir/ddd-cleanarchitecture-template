namespace DDDTemplate.Domain.SeedWork
{
    public interface IMongoRepository<TEntity> : IReadRepository<TEntity>, IWriteAsyncRepository<TEntity> where TEntity : class, IEntity
    {

    }
}
