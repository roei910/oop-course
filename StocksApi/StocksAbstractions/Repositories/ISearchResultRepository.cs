using StocksAbstractions.Models.SearchResults;

namespace StocksAbstractions.Repositories
{
    public interface ISearchResultRepository
    {
        Task<List<StockSearchResult>> SearchStockByTermAsync(string searchTerm);
    }
}
