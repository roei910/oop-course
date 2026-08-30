using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceGrid.FinancialData.Infrastructure.Persistence.Repositories;

public class StockHistoryRepository : IStockHistoryRepository
{
    private readonly IDbContextFactory<FinancialDataDbContext> _contextFactory;

    public StockHistoryRepository(IDbContextFactory<FinancialDataDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task AddHistoryEntriesAsync(List<StockHistoryEntry> entries)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.StockHistory.AddRange(entries);
        await context.SaveChangesAsync();
    }

    public async Task UpdateStocksHistoryAsync(List<Stock> stocks, DateOnly date)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var entries = new List<StockHistoryEntry>();
        foreach (var stock in stocks)
        {
            entries.Add(new StockHistoryEntry
            {
                StockSymbol = stock.Symbol,
                Date = date,
                PriceOpen = stock.RegularMarketOpen,
                PriceClose = stock.Price,
                DayLow = stock.RegularMarketDayLow,
                DayHigh = stock.RegularMarketDayHigh,
                DayRange = stock.RegularMarketDayRange ?? string.Empty,
                DayVolume = stock.RegularMarketVolume
            });
        }
        context.StockHistory.AddRange(entries);
        await context.SaveChangesAsync();
    }
}
