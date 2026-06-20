using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public interface IYahooFinance
{
    Task<List<StockSearchResult>> FindStockAsync(string searchTerm);
    Task<Stock?> GetStockAsync(string symbol);
}
