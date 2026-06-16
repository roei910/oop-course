using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;

namespace FinanceGrid.FinancialData.Application.Services;

public class SearchService : ISearchService
{
    private readonly ISearchResultRepository _searchResultRepository;
    private readonly IFinanceStrategy _financeStrategy;

    public SearchService(
        ISearchResultRepository searchResultRepository,
        IFinanceStrategy financeStrategy)
    {
        _searchResultRepository = searchResultRepository;
        _financeStrategy = financeStrategy;
    }

    public async Task<List<StockSearchResult>> SearchStockByTermAsync(string searchTerm)
    {
        var cached = await _searchResultRepository.SearchStockByTermAsync(searchTerm);
        if (cached.Count > 0)
            return cached;

        var results = await _financeStrategy.FindStockAsync(searchTerm);

        var searchResult = new SearchResult
        {
            Id = Guid.NewGuid().ToString(),
            SearchTerm = searchTerm,
            StockSearchResults = results
        };
        await _searchResultRepository.CreateAsync(searchResult);

        return results;
    }
}
