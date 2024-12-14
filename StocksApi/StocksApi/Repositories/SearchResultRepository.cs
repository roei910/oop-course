using Library.Models.SearchResults;
using StocksApi.Generators;
using StocksApi.Interfaces;
using YahooFinance.Interfaces;

namespace StocksApi.Repositories
{
    public class SearchResultRepository : ISearchResultRepository
    {
        private readonly ISearchResultsDal _searchResultsDal;
        private readonly ILogger<SearchResult> _logger;
        private readonly IFinanceStrategy _yahooFinanceStrategy;

        public SearchResultRepository(
            ISearchResultsDal stocksDal,
            ILogger<SearchResult> logger,
            IFinanceStrategy yahooFinanceStrategy)
        {
            _searchResultsDal = stocksDal;
            _logger = logger;
            _yahooFinanceStrategy = yahooFinanceStrategy;
        }

        public async Task<List<StockSearchResult>> SearchStockByTermAsync(string searchTerm)
        {
            var findSearchResult = await _searchResultsDal.FindBySearchTermAsync(searchTerm);

            if (findSearchResult is null)
            {
                var stockSearchResults = await CreateSearchResultAsync(searchTerm);

                return stockSearchResults;
            }

            return findSearchResult.StockSearchResults;
        }

        private async Task<List<StockSearchResult>> CreateSearchResultAsync(string searchTerm)
        {
            var stockSearchResults = await _yahooFinanceStrategy.FindStockAsync(searchTerm);

            if (stockSearchResults is null)
            {
                _logger.LogWarning("search term stock search results returned null {searchTerm}", searchTerm);

                stockSearchResults = new List<StockSearchResult>();
            }

            var searchResult = SearchResultGenerator.Generate(searchTerm, stockSearchResults);

            await _searchResultsDal.CreateAsync(searchResult);

            return stockSearchResults;
        }
    }
}