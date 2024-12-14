using YahooFinance.Models.RealTimeFinanceData;

namespace YahooFinance.Interfaces
{
    public interface IRealTimeFinanceData
    {
        Task<MarketTrendsResponse?> GetMarketTrendAsync(string trendType);
    }
}