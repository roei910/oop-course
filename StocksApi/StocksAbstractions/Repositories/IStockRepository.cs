using StocksAbstractions.Models;
using UsersAbstractions.Models.Notifications;

namespace StocksAbstractions.Repositories
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
