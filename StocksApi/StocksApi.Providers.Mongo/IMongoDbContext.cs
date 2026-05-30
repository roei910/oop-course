using MongoDB.Driver;

namespace StocksApi.Providers.Mongo
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
