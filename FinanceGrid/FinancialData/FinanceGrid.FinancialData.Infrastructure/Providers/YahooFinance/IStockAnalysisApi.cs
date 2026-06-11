using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public interface IStockAnalysisApi
{
    Task<StockAnalysis?> GetStockAnalysisAsync(string symbol);
    Task<List<StockAnalysis>> GetStocksAnalysisAsync(string[] symbols);
}
