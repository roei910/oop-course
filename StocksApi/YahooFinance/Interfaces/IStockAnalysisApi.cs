using Library.Models.Stocks;

namespace YahooFinance.Interfaces
{
	public interface IStockAnalysisApi
	{
        Task<StockAnalysis?> GetStockAnalysisAsync(string symbol);
        Task<List<StockAnalysis>> GetStocksAnalysisAsync(params string[] symbols);
    }
}