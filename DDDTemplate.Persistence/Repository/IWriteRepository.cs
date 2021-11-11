using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DDDTemplate.Domain.Shared;

namespace DDDTemplate.Persistence.Repository
{
    public interface IWriteRepository<TEntity> where TEntity : class, IEntity
    {
        void Insert(TEntity entity);
        Task InsertAsync(TEntity entity, CancellationToken token = default(CancellationToken));

        void InsertAll(IEnumerable<TEntity> entities);
        Task InsertAllAsync(IEnumerable<TEntity> entities, CancellationToken token = default(CancellationToken));

        void Update(TEntity entity);
        void UpdateEntry(TEntity entity);
        Task UpdateAsync(TEntity entity, CancellationToken token = default(CancellationToken));

        void Delete(Guid id);
        Task DeleteAsync(Guid id, CancellationToken token = default(CancellationToken));
    }
}
