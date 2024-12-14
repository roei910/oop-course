using Library.Models.SearchResults;

namespace StocksApi.Interfaces
{
    public interface ISearchResultRepository
    {
        Task<List<StockSearchResult>> SearchStockByTermAsync(string searchTerm);
    }
}