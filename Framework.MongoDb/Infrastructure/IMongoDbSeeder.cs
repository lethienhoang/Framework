using System.Threading.Tasks;

namespace Framework.MongoDb
{
    public interface IMongoDbSeeder
    {
        Task SeedAsync();
    }
}
