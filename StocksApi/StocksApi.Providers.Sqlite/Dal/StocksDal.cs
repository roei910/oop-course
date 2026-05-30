using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StocksAbstractions.Dal;
using StocksAbstractions.Models;
using StocksAbstractions.Models.Price;
using StocksAbstractions.Models.Stocks;
using UsersAbstractions.Models.Notifications;

namespace StocksApi.Providers.Sqlite.Dal
{
    public class StocksDal : IStocksDal
    {
        private readonly IDbContextFactory<SqliteDbContext> _contextFactory;
        private readonly ILogger<StocksDal> _logger;

        public StocksDal(IDbContextFactory<SqliteDbContext> contextFactory, ILogger<StocksDal> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<List<Stock>> FindAllAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Stocks.ToListAsync();
        }

        public async Task<Stock?> FindByIdAsync(string id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Stocks.FindAsync(id);
        }

        public async Task<Stock?> FindBySymbolAsync(string symbol)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task CreateAsync(Stock stock)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Stocks.Add(stock);
            await context.SaveChangesAsync();
        }

        public async Task CreateAsync(List<Stock> stocks)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Stocks.AddRange(stocks);
            await context.SaveChangesAsync();
        }

        public async Task RemoveAsync(string id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var stock = await context.Stocks.FindAsync(id);
            if (stock is not null)
            {
                context.Stocks.Remove(stock);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAnalysisBulkAsync(List<StockAnalysis> stocksAnalysis)
        {
            var symbols = stocksAnalysis.Select(a => a.Symbol).Distinct().ToList();

            await using var context = await _contextFactory.CreateDbContextAsync();

            var stocks = await context.Stocks
                .Where(s => symbols.Contains(s.Symbol))
                .ToListAsync();

            foreach (var stock in stocks)
            {
                var analysis = stocksAnalysis.FirstOrDefault(a => a.Symbol == stock.Symbol);
                if (analysis is not null)
                {
                    stock.Analysis = analysis;
                    stock.UpdatedTime = DateTime.UtcNow;
                }
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "bulk write exception, {count} entries", symbols.Count);
            }
        }

        public async Task UpdateStockPriceBulkAsync(List<PriceResponse> stockPriceResponses)
        {
            var symbols = stockPriceResponses.Select(r => r.Symbol).Distinct().ToList();

            await using var context = await _contextFactory.CreateDbContextAsync();

            var stocks = await context.Stocks
                .Where(s => symbols.Contains(s.Symbol))
                .ToListAsync();

            foreach (var stock in stocks)
            {
                var price = stockPriceResponses.FirstOrDefault(r => r.Symbol == stock.Symbol);
                if (price is null)
                    continue;

                stock.Price = price.RegularMarketPrice;
                stock.RegularMarketChange = price.RegularMarketChange;
                stock.RegularMarketChangePercent = price.RegularMarketChangePercent;
                stock.RegularMarketDayHigh = price.RegularMarketDayHigh;
                stock.RegularMarketDayLow = price.RegularMarketDayLow;
                stock.RegularMarketDayRange = price.RegularMarketDayRange;
                stock.RegularMarketOpen = price.RegularMarketOpen;
                stock.RegularMarketPreviousClose = price.RegularMarketPreviousClose;
                stock.RegularMarketVolume = price.RegularMarketVolume;
                stock.FiftyDayAverage = price.FiftyDayAverage;
                stock.FiftyTwoWeekHigh = price.FiftyTwoWeekHigh;
                stock.FiftyTwoWeekLow = price.FiftyTwoWeekLow;
                stock.FiftyTwoWeekRange = price.FiftyTwoWeekRange;
                stock.TwoHundredDayAverage = price.TwoHundredDayAverage;
                stock.TargetPriceHigh = price.TargetPriceHigh;
                stock.TargetPriceLow = price.TargetPriceLow;
                stock.TargetPriceMean = price.TargetPriceMean;
                stock.TargetPriceMedian = price.TargetPriceMedian;
                stock.ForwardPE = price.ForwardPE;
                stock.EpsCurrentYear = price.EpsCurrentYear;
                stock.EpsForward = price.EpsForward;
                stock.AnalystRating = price.AverageAnalystRating;
                stock.UpdatedTime = DateTime.UtcNow;
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "bulk write exception, {count} entries", symbols.Count);
            }
        }

        public async Task<List<Stock>> FindManyBySymbolAsync(string[] symbols)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Stocks
                .Where(s => symbols.Contains(s.Symbol))
                .ToListAsync();
        }

        public Task AddNotificationAsync(StockNotification stockNotification)
        {
            throw new NotSupportedException("Notifications are managed by UsersSqlite provider");
        }

        public Task RemoveNotificationAsync(string symbol, string notificationId)
        {
            throw new NotSupportedException("Notifications are managed by UsersSqlite provider");
        }
    }
}
