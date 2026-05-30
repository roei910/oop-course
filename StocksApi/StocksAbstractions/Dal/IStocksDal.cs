using StocksAbstractions.Models;
using StocksAbstractions.Models.Stocks;
using UsersAbstractions.Models.Notifications;
using StocksAbstractions.Models.Price;

namespace StocksAbstractions.Dal
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
