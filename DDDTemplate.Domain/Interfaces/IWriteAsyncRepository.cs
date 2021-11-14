using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DDDTemplate.Domain.Interfaces
{
    public interface IWriteAsyncRepository<TEntity> where TEntity : class, IEntity
    {
        Task InsertAsync(TEntity entity, CancellationToken token = default(CancellationToken));
        Task InsertAllAsync(IEnumerable<TEntity> entities, CancellationToken token = default(CancellationToken));
        Task UpdateAsync(TEntity entity, CancellationToken token = default(CancellationToken));
        Task DeleteAsync(Guid id, CancellationToken token = default(CancellationToken));
    }
}
