using StocksProvider.Models.RealTimeFinanceData;

namespace StocksProvider.Interfaces
{
    public interface IRealTimeFinanceData
    {
        Task<MarketTrendsResponse?> GetMarketTrendAsync(string trendType);
    }
}