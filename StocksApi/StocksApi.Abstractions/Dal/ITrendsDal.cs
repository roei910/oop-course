using StocksApi.Abstractions.Models;

namespace StocksApi.Abstractions.Dal
{
    public interface ITrendsDal
    {
        Task CreateAsync(MarketTrend trend);
        Task<List<MarketTrend>> FindAsync();
        Task<MarketTrend?> FindOneAsync(string trendName);
        Task RemoveAsync(string trendName);
        Task AddOrUpdateOneAsync(string trendName, MarketTrend marketTrend);
    }
}
