using StocksAbstractions.Models.SearchResults;
using StocksAbstractions.Models.Stocks;
using StocksAbstractions.Models.Price;

namespace StocksProvider.Interfaces
{
    public interface IFinanceStrategy
    {
        Task<List<StockSearchResult>> FindStockAsync(string searchTerm);
        Task<PriceResponse?> GetStockAsync(string symbol);
        Task<List<StockAnalysis>> GetStocksAnalysisAsync(string[] symbols);
        Task<List<PriceResponse>> GetStocksAsync(params string[] symbols);
    }
}