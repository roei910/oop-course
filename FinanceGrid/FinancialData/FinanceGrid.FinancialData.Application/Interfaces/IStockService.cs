using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Application.Interfaces;

public interface IStockService
{
    Task<List<Stock>> GetAllStocksAsync();
    Task<Stock?> GetStockBySymbolAsync(string symbol);
    Task<List<Stock>> GetStocksBySymbolsAsync(string[] symbols);
    Task UpdateStocksBySymbolAsync(string[] stockSymbols);
    Task UpdateStocksAnalysisAsync(string[] orderedStockSymbols);
    Task ForceUpdateAllStocksAsync();
}
