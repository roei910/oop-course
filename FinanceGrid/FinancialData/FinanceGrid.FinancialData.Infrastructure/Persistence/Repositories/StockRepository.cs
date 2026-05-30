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

    public async Task UpdateStocksBySymbolAsync(string[] stockSymbols)
    {
        throw new NotSupportedException("Use IStockService for updates that require external API calls");
    }

    public async Task UpdateStocksAnalysisAsync(string[] orderedStockSymbols)
    {
        throw new NotSupportedException("Use IStockService for analysis updates that require external API calls");
    }

    public async Task RemoveNotificationAsync(string symbol, string notificationId)
    {
        throw new NotSupportedException("Notifications are managed by the Webhook service");
    }

    public async Task AddNotificationAsync(StockNotification stockNotification)
    {
        throw new NotSupportedException("Notifications are managed by the Webhook service");
    }
}
