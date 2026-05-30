using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Application.Interfaces;

public interface IRealTimeFinanceData
{
    Task<MarketTrend?> GetMarketTrendAsync(string trendType);
}
