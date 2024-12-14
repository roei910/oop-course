using Library.Models.MarketTrends;

namespace StocksApi.Interfaces
{
	public interface ITrendRepository
	{
        Task<List<MarketTrend>> GetMarketTrendsAsync();
        Task<MarketTrend?> GetTrendAsync(string trendType);
    }
}