using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Domain.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync();
    Task<Stock?> GetStockBySymbolAsync(string symbol);
    Task<List<Stock>> GetStocksBySymbolAsync(string[] symbols);
    Task UpdateStocksAsync(List<Stock> stocks);
    Task UpdateStocksAnalysisAsync(List<StockAnalysis> analyses);
}
