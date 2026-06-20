using Microsoft.EntityFrameworkCore;
using StocksAbstractions.Dal;
using StocksAbstractions.Models;

namespace StocksApi.Providers.Sqlite.Dal
{
    public class TrendsDal : ITrendsDal
    {
        private readonly IDbContextFactory<SqliteDbContext> _contextFactory;

        public TrendsDal(IDbContextFactory<SqliteDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(MarketTrend trend)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.MarketTrends.Add(trend);
            await context.SaveChangesAsync();
        }

        public async Task<List<MarketTrend>> FindAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.MarketTrends
                .Include(t => t.TrendingStocks)
                .Include(t => t.StockNews)
                    .ThenInclude(n => n.StocksInNews)
                .ToListAsync();
        }

        public async Task<MarketTrend?> FindOneAsync(string trendName)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.MarketTrends
                .Include(t => t.TrendingStocks)
                .Include(t => t.StockNews)
                    .ThenInclude(n => n.StocksInNews)
                .FirstOrDefaultAsync(t => t.TrendName == trendName);
        }

        public async Task RemoveAsync(string trendName)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var trend = await context.MarketTrends
                .FirstOrDefaultAsync(t => t.TrendName == trendName);
            if (trend is not null)
            {
                context.MarketTrends.Remove(trend);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddOrUpdateOneAsync(string trendName, MarketTrend updatedMarketTrend)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var existing = await context.MarketTrends
                .Include(t => t.TrendingStocks)
                .Include(t => t.StockNews)
                .FirstOrDefaultAsync(t => t.TrendName == trendName);

            if (existing is null)
            {
                updatedMarketTrend.Id = Guid.NewGuid().ToString();
                context.MarketTrends.Add(updatedMarketTrend);
            }
            else
            {
                if (updatedMarketTrend.StockNews.Count > 0)
                {
                    context.StockNews.RemoveRange(existing.StockNews);
                    existing.StockNews = updatedMarketTrend.StockNews;
                }

                if (updatedMarketTrend.TrendingStocks.Count > 0)
                {
                    context.StockTrends.RemoveRange(existing.TrendingStocks);
                    existing.TrendingStocks = updatedMarketTrend.TrendingStocks;
                }

                existing.LastUpdatedTime = updatedMarketTrend.LastUpdatedTime;
            }

            await context.SaveChangesAsync();
        }
    }
}
