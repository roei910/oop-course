using Library.Interfaces;
using Library.Models;
using Library.Models.MarketTrends;
using MongoDB.Driver;
using StocksApi.Interfaces;

namespace StocksApi.Dal
{
	public class TrendsDal : ITrendsDal
	{
        private readonly IMongoCollection<MarketTrend> _collection;

        public TrendsDal(IAppConfiguration appConfiguration)
        {
            var databaseSettings = appConfiguration
                .Get<DatabaseSettings>(ConfigurationKeys.DatabaseSettingsSection);

            var connectionString = appConfiguration.Get<string>(ConfigurationKeys.ConnectionStringSection);

            var mongoClient = new MongoClient(connectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.DatabaseName);

            _collection = mongoDatabase.GetCollection<MarketTrend>(
                databaseSettings.MarketTrendsCollectionName);
        }

        public async Task<List<MarketTrend>> FindAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<MarketTrend?> FindOneAsync(string trendName)
        {
            var filter = Builders<MarketTrend>.Filter
                .Eq(trend => trend.TrendName, trendName);

            var foundTrend = await (await _collection
                .FindAsync(filter)).FirstOrDefaultAsync();

            return foundTrend;
        }

        public async Task CreateAsync(MarketTrend trend) =>
            await _collection.InsertOneAsync(trend);

        public async Task RemoveAsync(string trendName) =>
            await _collection.DeleteOneAsync(x => x.TrendName == trendName);

        public async Task AddOrUpdateOneAsync(string trendName, MarketTrend updatedMarketTrend)
        {
            var filter = Builders<MarketTrend>.Filter
                .Eq(trend => trend.TrendName, trendName);

            var cursor = await _collection.FindAsync(filter);
            var foundMarketTrend = await cursor.FirstOrDefaultAsync();

            if (foundMarketTrend is null)
            {
                await _collection.InsertOneAsync(updatedMarketTrend);

                return;
            }

            var update = Builders<MarketTrend>.Update
                .Set(marketTrend => marketTrend.LastUpdatedTime, updatedMarketTrend.LastUpdatedTime);

            if (updatedMarketTrend.StockNews.Count > 0)
                update = update.Set(marketTrend => marketTrend.StockNews, updatedMarketTrend.StockNews);

            if (updatedMarketTrend.TrendingStocks.Count > 0)
                update = update.Set(marketTrend => marketTrend.TrendingStocks, updatedMarketTrend.TrendingStocks);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}

