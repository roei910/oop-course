using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FinanceGrid.Shared;

namespace FinanceGrid.FinancialData.Infrastructure.Persistence.Repositories;

public class TrendRepository : ITrendRepository
{
    private readonly IDbContextFactory<FinancialDataDbContext> _contextFactory;
    private readonly ILogger<TrendRepository> _logger;

    public TrendRepository(
        IDbContextFactory<FinancialDataDbContext> contextFactory,
        ILogger<TrendRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<List<MarketTrend>> GetMarketTrendsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MarketTrends
            .Include(m => m.TrendingStocks)
            .Include(m => m.StockNewsItems)
                .ThenInclude(n => n.StocksInNews)
            .ToListAsync();
    }

    public async Task<MarketTrend?> GetTrendAsync(string trendType)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MarketTrends
            .Include(m => m.TrendingStocks)
            .Include(m => m.StockNewsItems)
                .ThenInclude(n => n.StocksInNews)
            .FirstOrDefaultAsync(m => m.TrendName == trendType);
    }
}
