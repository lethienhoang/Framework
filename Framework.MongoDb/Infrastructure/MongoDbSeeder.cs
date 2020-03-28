using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.MongoDb
{
    public class MongoDbSeeder : IMongoDbSeeder
    {
        protected readonly IMongoDatabase _database;

        public MongoDbSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task SeedAsync()
        {
            await CustomSeedAsync();
        }

        protected virtual async Task CustomSeedAsync()
        {
            var cursor = await _database.ListCollectionsAsync();
            var collections = await cursor.ToListAsync();
            if (collections.Any())
            {
                return;
            }

            await Task.CompletedTask;
        }
    }
}
