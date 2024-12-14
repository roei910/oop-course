using Library.Models;
using Library.Models.Shares;
using Library.Models.Users;

namespace StocksApi.Interfaces
{
    public interface IUserRepository
    {
        Task AddNotificationAsync(StockNotification notification);
        Task<Share> AddShareAsync(string id, SharePurchase share);
        Task AddStockListAsync(StockListDetails stockListDetails);
        Task AddUserAsync(UserDetails userDetails);
        Task AddWatchingStockAsync(string id, string listName, string stockSymbol);
        Task<bool> ConnectUserAsync(UserCredentials user);
        Task RemoveNotificationAsync(string id, string notificationId);
        Task<List<User>> GetAllAsync();
        Task<User?> GetAsync(string email);
        Task RemoveShareAsync(ShareSale shareSale);
        Task RemoveStockListAsync(StockListDetails stockListDetails);
        Task RemoveUserAsync(string id);
        Task RemoveWatchingStockAsync(string id, string listName, string stockSymbol);
        Task ShowNotificationAsync(StockNotification notification);
        Task UpdateShareNoteAsync(WatchingStockAction watchingStockAction);
        Task UpdatePasswordAsync(PasswordUpdateRequest passwordUpdateRequest);
    }
}