using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;
using FinanceGrid.FinancialData.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Persistence.Repositories;

public class TrendRepositoryExtended : ITrendRepository
{
    private readonly TrendRepository _dbRepo;
    private readonly IRealTimeFinanceData _realTimeFinanceData;
    private readonly ILogger<TrendRepositoryExtended> _logger;
    private readonly List<string> _trendTypes;

    public TrendRepositoryExtended(
        TrendRepository dbRepo,
        IRealTimeFinanceData realTimeFinanceData,
        ILogger<TrendRepositoryExtended> logger)
    {
        _dbRepo = dbRepo;
        _realTimeFinanceData = realTimeFinanceData;
        _logger = logger;
        _trendTypes = ["MOST_ACTIVE", "GAINERS", "LOSERS", "CLIMATE_LEADERS"];
    }

    public async Task<List<MarketTrend>> GetMarketTrendsAsync()
    {
        var tasks = _trendTypes.Select(GetTrendAsync).ToList();
        await Task.WhenAll(tasks);
        return tasks.Where(t => t.Result is not null).Select(t => t.Result!).ToList();
    }

    public async Task<MarketTrend?> GetTrendAsync(string trendType)
    {
        var trend = await _dbRepo.GetTrendAsync(trendType);

        if (trend is not null)
        {
            var passedTimeSinceUpdate = DateTime.UtcNow - trend.LastUpdatedTime;
            if (passedTimeSinceUpdate.Days < 2)
                return trend;
        }

        var marketTrendResponse = await _realTimeFinanceData.GetMarketTrendAsync(trendType);

        if (marketTrendResponse is null)
            return trend;

        marketTrendResponse.TrendName = trendType;
        marketTrendResponse.LastUpdatedTime = DateTime.UtcNow;

        _logger.LogInformation("Updated market trend, {TrendName}", trendType);
        return marketTrendResponse;
    }
}
