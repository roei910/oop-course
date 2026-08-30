using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Domain.Interfaces;

public interface IStockHistoryRepository
{
    Task AddHistoryEntriesAsync(List<StockHistoryEntry> entries);
    Task UpdateStocksHistoryAsync(List<Stock> stocks, DateOnly date);
}
