namespace DDDTemplate.Domain.Interfaces
{
    public interface IMongoRepository<TEntity> : IReadRepository<TEntity>, IWriteAsyncRepository<TEntity> where TEntity : class, IEntity
    {

    }
}
