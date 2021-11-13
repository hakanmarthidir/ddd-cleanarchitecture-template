using System;
using System.Collections.Generic;

namespace DDDTemplate.Domain.SeedWork
{
    public interface IWriteRepository<TEntity> where TEntity : class, IEntity
    {
        void Insert(TEntity entity);
        void InsertAll(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(Guid id);
    }
}
