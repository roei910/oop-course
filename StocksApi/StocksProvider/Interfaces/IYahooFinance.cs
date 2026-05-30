using StocksAbstractions.Models.SearchResults;
using StocksAbstractions.Models.Price;

namespace StocksProvider.Interfaces
{
    public interface IYahooFinance
    {
        Task<PriceResponse?> GetStockAsync(string symbol);
        Task<List<StockSearchResult>> FindStockAsync(string searchTerm);
    }
}