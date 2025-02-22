using Library.Models.SearchResults;
using YahooFinance.Models.Price;

namespace YahooFinance.Interfaces
{
    public interface IYahooFinance
    {
        Task<PriceResponse?> GetStockAsync(string symbol);
        Task<List<StockSearchResult>> FindStockAsync(string searchTerm);
    }
}