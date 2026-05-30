using StocksApi.Providers.Mongo;
using StocksAbstractions.Models;
using MongoDB.Driver;
using StocksAbstractions.Dal;

namespace StocksApi.Providers.Mongo.Dal
{
	public class TrendsDal : ITrendsDal
	{
        private readonly IMongoCollection<MarketTrend> _collection;

        public TrendsDal(IMongoDbContext mongoDbContext)
        {
            _collection = mongoDbContext.GetCollection<MarketTrend>("MarketTrends");
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

            var update = Builders<MarketTrend>.Update
                .SetOnInsert(marketTrend => marketTrend.TrendingStocks, updatedMarketTrend.TrendingStocks)
                .SetOnInsert(marketTrend => marketTrend.StockNews, updatedMarketTrend.StockNews)
                .Set(marketTrend => marketTrend.LastUpdatedTime, updatedMarketTrend.LastUpdatedTime);

            if (updatedMarketTrend.StockNews.Count > 0)
                update = update.Set(marketTrend => marketTrend.StockNews, updatedMarketTrend.StockNews);

            if (updatedMarketTrend.TrendingStocks.Count > 0)
                update = update.Set(marketTrend => marketTrend.TrendingStocks, updatedMarketTrend.TrendingStocks);

            await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
    }
}
