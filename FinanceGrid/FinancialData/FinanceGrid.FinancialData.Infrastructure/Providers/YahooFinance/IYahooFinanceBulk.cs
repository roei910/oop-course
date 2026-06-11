using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public interface IYahooFinanceBulk
{
    Task<List<Stock>> GetStocksAsync(string[] symbols);
}
