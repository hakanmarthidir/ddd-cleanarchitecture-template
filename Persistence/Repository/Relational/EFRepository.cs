using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.Relational;
using System.Linq.Expressions;

namespace Persistence.Repository.Relational
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot, new()
    {
        protected readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbset;

        public EFRepository(EFContext mainContext)
        {
            _dbContext = mainContext ?? throw new ArgumentNullException("Database Context can not be null.");
            _dbset = _dbContext.Set<TEntity>();
        }

        public ICollection<TEntity> Find(ISpec<TEntity> spec)
        {
            var query = this.SetSpec(spec);
            return query.ToList();
        }

        public async Task<ICollection<TEntity>> FindAsync(ISpec<TEntity> spec, CancellationToken token = default(CancellationToken))
        {
            var query = this.SetSpec(spec);
            return await query.ToListAsync(token).ConfigureAwait(false);
        }

        public TEntity FindById<TId>(TId id)
        {
            return _dbset.Find(new object[] { id });
        }

        public async Task<TEntity> FindByIdAsync<TId>(TId id, CancellationToken token = default(CancellationToken))
        {
            return await _dbset.FindAsync(new object[] { id }, token).ConfigureAwait(false);
        }

        public void Insert(TEntity entity)
        {
            _dbset.Add(entity);

        }

        public async Task InsertAsync(TEntity entity, CancellationToken token = default(CancellationToken))
        {
            await _dbset.AddAsync(entity, token).ConfigureAwait(false);
        }

        public void InsertAll(IEnumerable<TEntity> entities)
        {
            this._dbset.AddRange(entities);
        }

        public async Task InsertAllAsync(IEnumerable<TEntity> entities, CancellationToken token = default(CancellationToken))
        {
            await _dbset.AddRangeAsync(entities, token).ConfigureAwait(false);
        }

        public void Update(TEntity entity)
        {
            _dbset.Update(entity);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken token = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                _dbset.Update(entity);
            }).ConfigureAwait(false);
        }

        public void UpdateEntry(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete<TId>(TId id)
        {
            var deletedItem = FindById<TId>(id);

            if (deletedItem != null)
            {
                if (deletedItem is ISoftDeletable e)
                {
                    e.SoftDelete();
                    this.Update(deletedItem);
                }
                else
                {
                    this.Remove(deletedItem);
                }
            }

        }

        public async Task DeleteAsync<TId>(TId id, CancellationToken token = default(CancellationToken))
        {
            var deletedItem = await FindByIdAsync<TId>(id, token);

            if (deletedItem != null)
            {
                if (deletedItem is ISoftDeletable e)
                {
                    e.SoftDelete();
                    await this.UpdateAsync(deletedItem);
                }
                else
                {
                    await this.RemoveAsync(deletedItem);
                }
            }
        }

        private void Remove(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        private async Task RemoveAsync(TEntity entity)
        {
            await Task.Run(() => _dbset.Remove(entity)).ConfigureAwait(false);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken))
        {
            return await this._dbset.FirstOrDefaultAsync(predicate, token).ConfigureAwait(false);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this._dbset.FirstOrDefault(predicate);
        }

        private IQueryable<TEntity> SetSpec(ISpec<TEntity> specification)
        {
            return SpecHandler<TEntity>.GetQuery(this._dbset.AsQueryable(), specification);
        }
    }
}
