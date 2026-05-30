using StocksApi.Abstractions.Models;
using StocksApi.Abstractions.Models.Stocks;
using StocksApi.Abstractions.Models.Users.Notifications;
using StocksApi.Abstractions.Models.Price;

namespace StocksApi.Abstractions.Dal
{
    public interface IStocksDal
	{
        Task<List<Stock>> FindAllAsync();
        Task<Stock?> FindByIdAsync(string id);
        Task<Stock?> FindBySymbolAsync(string symbol);
        Task CreateAsync(Stock stock);
        Task CreateAsync(List<Stock> stocks);
        Task RemoveAsync(string id);
        Task UpdateAnalysisBulkAsync(List<StockAnalysis> stocksAnalysis);
        Task UpdateStockPriceBulkAsync(List<PriceResponse> stockPriceResponses);
        Task<List<Stock>> FindManyBySymbolAsync(string[] symbols);
        Task AddNotificationAsync(StockNotification stockNotification);
        Task RemoveNotificationAsync(string symbol, string notificationId);
    }
}
