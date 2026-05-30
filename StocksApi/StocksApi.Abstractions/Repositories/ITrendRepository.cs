using StocksApi.Abstractions.Models;

namespace StocksApi.Abstractions.Repositories
{
	public interface ITrendRepository
	{
        Task<List<MarketTrend>> GetMarketTrendsAsync();
        Task<MarketTrend?> GetTrendAsync(string trendType);
    }
}
