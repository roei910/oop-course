using StocksAbstractions.Models.Stocks;

namespace StocksProvider.Interfaces
{
	public interface IStockAnalysisApi
	{
        Task<StockAnalysis?> GetStockAnalysisAsync(string symbol);
        Task<List<StockAnalysis>> GetStocksAnalysisAsync(params string[] symbols);
    }
}