namespace DDDTemplate.Domain.Interfaces
{
    public interface IEFRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity>, IWriteAsyncRepository<TEntity> where TEntity : class, IEntity
    {

    }
}
