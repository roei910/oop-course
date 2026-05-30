using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Application.Interfaces;

public interface IFinanceStrategy
{
    Task<Stock?> GetStockAsync(string symbol);
    Task<List<Stock>?> GetStocksAsync(string symbols);
    Task<List<StockSearchResult>> FindStockAsync(string searchTerm);
    Task<StockAnalysis?> GetStockAnalysisAsync(string symbol);
    Task<List<StockAnalysis>> GetStocksAnalysisAsync(string[] symbols);
}
