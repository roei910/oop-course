using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;

namespace FinanceGrid.FinancialData.Application.Services;

public class TrendService : ITrendService
{
    private readonly ITrendRepository _trendRepository;

    public TrendService(ITrendRepository trendRepository)
    {
        _trendRepository = trendRepository;
    }

    public async Task<List<MarketTrend>> GetMarketTrendsAsync()
    {
        return await _trendRepository.GetMarketTrendsAsync();
    }
}
