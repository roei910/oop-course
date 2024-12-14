using Library.Models;
using Library.Models.Stocks;
using Library.Models.Users;
using YahooFinance.Models;

namespace StocksApi.Interfaces
{
	public interface IStocksDal
	{
        Task<List<Stock>> FindAllAsync();
        Task<Stock?> FindByIdAsync(string id);
        Task<Stock?> FindBySymbolAsync(string symbol);
        Task CreateAsync(Stock stock);
        Task UpdateAsync(string id, Stock updatedStock);
        Task RemoveAsync(string id);
        Task UpdateAnalysisBulkAsync(List<StockAnalysis> stocksAnalysis);
        Task UpdateStockPriceBulkAsync(List<PriceResponse> stockPriceResponses);
        Task<List<Stock>> FindManyBySymbolAsync(string[] symbols);
        Task AddNotificationAsync(StockNotification stockNotification);
        Task RemoveNotificationAsync(string symbol, string notificationId);
    }
}