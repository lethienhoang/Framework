using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Domain;
using Framework.Types;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Framework.MongoDb
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : IIdentifiable
    {
        protected IMongoCollection<TEntity> Collection { get; }

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            Collection = database.GetCollection<TEntity>(collectionName);
        }

        public async Task AddAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        public Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.Find(predicate).AnyAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Collection.DeleteOneAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            return await Collection.Find(e => e.Id == id).SingleOrDefaultAsync();
        }

        public async Task<TEntity> GetAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).SingleOrDefaultAsync();
        }

        public async Task<PagedResult<TEntity>> SearchAsync<TQuery>(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, TQuery query) where TQuery : PagedQueryBase
        {
            return await Collection.AsQueryable().Where(predicate).PaginateAsync(query);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }
    }
}
