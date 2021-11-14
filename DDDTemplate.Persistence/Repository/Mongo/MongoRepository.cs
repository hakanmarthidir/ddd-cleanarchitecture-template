using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DDDTemplate.Domain.Enums;
using DDDTemplate.Domain.Interfaces;
using DDDTemplate.Persistence.Context.Mongo;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DDDTemplate.Persistence.Repository.Mongo
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : class, IEntity, new()
    {
        private readonly IMongoContext _dbContext;
        private IMongoCollection<TEntity> _collection;

        public MongoRepository(IMongoContext dbContext)
        {
            this._dbContext = dbContext;
            this._collection = _dbContext.Database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        #region Read

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = _collection.AsQueryable<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
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

        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = _collection.AsQueryable<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
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

        public TEntity FindById(Guid id)
        {
            return this._collection.Find<TEntity>(x => x.Id == id).SingleOrDefault();
        }

        public async Task<TEntity> FindByIdAsync(Guid id, CancellationToken token = default)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            var cursor = await this._collection.FindAsync<TEntity>(filter).ConfigureAwait(false);
            return await cursor.SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this._collection.Find<TEntity>(predicate).FirstOrDefault();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            var cursor = await this._collection.FindAsync<TEntity>(filter).ConfigureAwait(false);
            return await cursor.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        #endregion

        #region Write

        public async Task InsertAllAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
        {
            await this._collection.InsertManyAsync(entities, cancellationToken: token).ConfigureAwait(false);
        }

        public async Task InsertAsync(TEntity entity, CancellationToken token = default)
        {
            await this._collection.InsertOneAsync(entity, cancellationToken: token).ConfigureAwait(false);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken token = default)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            await this._collection.FindOneAndReplaceAsync(filter, entity, cancellationToken: token).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            var deletedItem = await this.FindByIdAsync(id, token);

            if (deletedItem != null)
            {
                if (deletedItem is ISoftDelete e)
                {
                    e.Status = Status.Deleted;
                    e.DeletedDate = DateTimeOffset.UtcNow;
                    await this.UpdateAsync(deletedItem, token);
                }
                else
                {
                    var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
                    await this._collection.DeleteOneAsync(filter, token).ConfigureAwait(false);
                }
            }
        }

        #endregion
    }
}
