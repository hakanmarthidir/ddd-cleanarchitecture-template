using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.Relational;
using System.Linq.Expressions;

namespace Persistence.Repository.Relational
{
    public class EFRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>, IAggregateRoot, new()
    {
        protected readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbset;

        public EFRepository(EFContext mainContext)
        {
            _dbContext = mainContext ?? throw new ArgumentNullException("Database Context can not be null.");
            _dbset = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query;

        }

        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.ToListAsync().ConfigureAwait(false);

        }

        public TEntity FindById(TKey id)
        {
            return _dbset.Find(new object[] { id });
        }

        public async Task<TEntity> FindByIdAsync(TKey id, CancellationToken token = default(CancellationToken))
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

        public void Delete(TKey id)
        {
            var deletedItem = FindById(id);

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

        public async Task DeleteAsync(TKey id, CancellationToken token = default(CancellationToken))
        {
            var deletedItem = await FindByIdAsync(id, token);

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

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
          => await this._dbset.FirstOrDefaultAsync(predicate).ConfigureAwait(false);

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
          => this._dbset.FirstOrDefault(predicate);

    }
}
