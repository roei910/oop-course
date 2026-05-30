using SharedLibrary.Models;
using SharedLibrary.Services;
using MongoDB.Driver;

namespace StocksApi.Providers.Mongo
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IAppConfiguration appConfiguration)
        {
            var databaseSettings = appConfiguration.Get<DatabaseSettings>(ConfigurationKeys.DatabaseSettingsSection);
            var connectionString = appConfiguration.Get<string>(ConfigurationKeys.ConnectionStringSection);
            var mongoClient = new MongoClient(connectionString);
            _database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name) =>
            _database.GetCollection<T>(name);
    }
}
