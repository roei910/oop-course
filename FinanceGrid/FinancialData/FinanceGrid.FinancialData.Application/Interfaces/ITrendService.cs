using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Application.Interfaces;

public interface ITrendService
{
    Task<List<MarketTrend>> GetMarketTrendsAsync();
}
