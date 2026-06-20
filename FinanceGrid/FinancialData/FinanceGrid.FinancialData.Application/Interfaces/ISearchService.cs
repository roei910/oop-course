using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Application.Interfaces;

public interface ISearchService
{
    Task<List<StockSearchResult>> SearchStockByTermAsync(string searchTerm);
}
