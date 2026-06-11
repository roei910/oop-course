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
}
