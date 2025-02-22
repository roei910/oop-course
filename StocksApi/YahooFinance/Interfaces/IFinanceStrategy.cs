using Library.Models.SearchResults;
using Library.Models.Stocks;
using YahooFinance.Models.Price;

namespace YahooFinance.Interfaces
{
    public interface IFinanceStrategy
    {
        Task<List<StockSearchResult>> FindStockAsync(string searchTerm);
        Task<PriceResponse?> GetStockAsync(string symbol);
        Task<List<StockAnalysis>> GetStocksAnalysisAsync(string[] symbols);
        Task<List<PriceResponse>> GetStocksAsync(params string[] symbols);
    }
}