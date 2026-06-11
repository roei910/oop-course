using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace FinanceGrid.FinancialData.Infrastructure.Persistence.Repositories;

public class StockRepository : IStockRepository
{
    private readonly IDbContextFactory<FinancialDataDbContext> _contextFactory;
    private readonly ILogger<StockRepository> _logger;

    public StockRepository(
        IDbContextFactory<FinancialDataDbContext> contextFactory,
        ILogger<StockRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<List<Stock>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Stocks.ToListAsync();
    }

    public async Task<Stock?> GetStockBySymbolAsync(string symbol)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<List<Stock>> GetStocksBySymbolAsync(string[] symbols)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Stocks.Where(s => symbols.Contains(s.Symbol)).ToListAsync();
    }

    public async Task UpdateStocksAsync(List<Stock> stocks)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        foreach (var stock in stocks)
        {
            var existing = await context.Stocks.FirstOrDefaultAsync(s => s.Symbol == stock.Symbol);
            if (existing is not null)
            {
                existing.Price = stock.Price;
                existing.RegularMarketPreviousClose = stock.RegularMarketPreviousClose;
                existing.RegularMarketOpen = stock.RegularMarketOpen;
                existing.RegularMarketDayLow = stock.RegularMarketDayLow;
                existing.RegularMarketDayHigh = stock.RegularMarketDayHigh;
                existing.RegularMarketDayRange = stock.RegularMarketDayRange;
                existing.RegularMarketChange = stock.RegularMarketChange;
                existing.RegularMarketChangePercent = stock.RegularMarketChangePercent;
                existing.RegularMarketVolume = stock.RegularMarketVolume;
                existing.FiftyDayAverage = stock.FiftyDayAverage;
                existing.TwoHundredDayAverage = stock.TwoHundredDayAverage;
                existing.FiftyTwoWeekRange = stock.FiftyTwoWeekRange;
                existing.FiftyTwoWeekLow = stock.FiftyTwoWeekLow;
                existing.FiftyTwoWeekHigh = stock.FiftyTwoWeekHigh;
                existing.TargetPriceLow = stock.TargetPriceLow;
                existing.TargetPriceHigh = stock.TargetPriceHigh;
                existing.TargetPriceMean = stock.TargetPriceMean;
                existing.TargetPriceMedian = stock.TargetPriceMedian;
                existing.ForwardPE = stock.ForwardPE;
                existing.EpsCurrentYear = stock.EpsCurrentYear;
                existing.EpsForward = stock.EpsForward;
                existing.FullExchangeName = stock.FullExchangeName;
                existing.AnalystRating = stock.AnalystRating;
                existing.UpdatedTime = DateTime.UtcNow;
            }
        }
        await context.SaveChangesAsync();
    }

    public async Task UpdateStocksAnalysisAsync(List<StockAnalysis> analyses)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        foreach (var analysis in analyses)
        {
            var stock = await context.Stocks.FirstOrDefaultAsync(s => s.Symbol == analysis.Symbol);
            if (stock is not null)
            {
                stock.Analysis = analysis;
                stock.Analysis.UpdatedTime = DateTime.UtcNow;
            }
        }
        await context.SaveChangesAsync();
    }
}
