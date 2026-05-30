using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StocksAbstractions.Dal;
using StocksAbstractions.Models;
using StocksAbstractions.Models.Stocks;

namespace StocksApi.Providers.Sqlite.Dal
{
    public class StockHistoriesDal : IStockHistoriesDal
    {
        private readonly IDbContextFactory<SqliteDbContext> _contextFactory;
        private readonly ILogger<StockHistoriesDal> _logger;

        public StockHistoriesDal(IDbContextFactory<SqliteDbContext> contextFactory, ILogger<StockHistoriesDal> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task UpdateStocksHistoryAsync(DateTime lastCloseDateTime)
        {
            var lastHistoryUpdateDate = DateOnly.FromDateTime(lastCloseDateTime);

            await using var context = await _contextFactory.CreateDbContextAsync();

            var stocksToUpdate = await context.Stocks
                .Where(s => s.LastHistoryUpdateDate == null || s.LastHistoryUpdateDate < lastHistoryUpdateDate)
                .Select(s => new
                {
                    s.Symbol,
                    s.Price,
                    s.RegularMarketOpen,
                    s.RegularMarketDayLow,
                    s.RegularMarketDayHigh,
                    s.RegularMarketVolume,
                    s.RegularMarketDayRange
                })
                .ToListAsync();

            if (stocksToUpdate.Count == 0)
                return;

            var historyEntries = new List<StockHistoryEntry>();

            foreach (var stock in stocksToUpdate)
            {
                historyEntries.Add(new StockHistoryEntry
                {
                    Id = Guid.NewGuid().ToString(),
                    StockSymbol = stock.Symbol,
                    Date = lastHistoryUpdateDate,
                    DayLow = stock.RegularMarketDayLow,
                    DayHigh = stock.RegularMarketDayHigh,
                    DayRange = stock.RegularMarketDayRange ?? "",
                    DayVolume = stock.RegularMarketVolume,
                    PriceClose = stock.Price,
                    PriceOpen = stock.RegularMarketOpen
                });
            }

            try
            {
                context.StockHistory.AddRange(historyEntries);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed to insert stock history, {count} entries", historyEntries.Count);
                return;
            }

            var symbols = stocksToUpdate.Select(s => s.Symbol).ToList();
            var stocks = await context.Stocks
                .Where(s => symbols.Contains(s.Symbol))
                .ToListAsync();

            foreach (var stock in stocks)
            {
                stock.LastHistoryUpdateDate = lastHistoryUpdateDate;
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed to update stock last history date, {count} updates", stocks.Count);
            }
        }
    }
}
