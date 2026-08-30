using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Domain.Interfaces;

public interface ISearchResultRepository
{
    Task<List<StockSearchResult>> SearchStockByTermAsync(string searchTerm);
    Task CreateAsync(SearchResult searchResult);
}
