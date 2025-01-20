using Library.Models;
using Library.Models.Users.Notifications;

namespace StocksApi.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetStockBySymbolAsync(string symbol);
        Task<List<Stock>> GetStocksBySymbolAsync(string[] symbol);
        Task UpdateStocksBySymbolAsync(string[] stockSymbols);
        Task UpdateStocksAnalysisAsync(string[] orderedStockSymbols);
        Task RemoveNotificationAsync(string symbol, string notificationId);
        Task AddNotificationAsync(StockNotification stockNotification);
    }
}