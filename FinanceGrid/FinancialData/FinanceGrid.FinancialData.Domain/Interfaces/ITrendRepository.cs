using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Domain.Interfaces;

public interface ITrendRepository
{
    Task<List<MarketTrend>> GetMarketTrendsAsync();
    Task<MarketTrend?> GetTrendAsync(string trendType);
}
