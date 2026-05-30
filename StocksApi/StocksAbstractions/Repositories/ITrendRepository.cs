using StocksAbstractions.Models;

namespace StocksAbstractions.Repositories
{
	public interface ITrendRepository
	{
        Task<List<MarketTrend>> GetMarketTrendsAsync();
        Task<MarketTrend?> GetTrendAsync(string trendType);
    }
}
