using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;

namespace FinanceGrid.FinancialData.Application.Services;

public class SearchService : ISearchService
{
    private readonly ISearchResultRepository _searchResultRepository;

    public SearchService(ISearchResultRepository searchResultRepository)
    {
        _searchResultRepository = searchResultRepository;
    }

    public async Task<List<StockSearchResult>> SearchStockByTermAsync(string searchTerm)
    {
        return await _searchResultRepository.SearchStockByTermAsync(searchTerm);
    }
}
