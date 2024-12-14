using Library.Models.MarketTrends;

namespace StocksApi.Interfaces
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