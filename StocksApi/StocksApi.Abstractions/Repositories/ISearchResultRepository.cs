using StocksApi.Abstractions.Models.SearchResults;

namespace StocksApi.Abstractions.Repositories
{
    public interface ISearchResultRepository
    {
        Task<List<StockSearchResult>> SearchStockByTermAsync(string searchTerm);
    }
}
