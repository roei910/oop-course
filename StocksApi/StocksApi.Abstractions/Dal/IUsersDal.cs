using StocksApi.Abstractions.Models;
using StocksApi.Abstractions.Models.Users;
using StocksApi.Abstractions.Models.Users.Notifications;
using StocksApi.Abstractions.Models.Users.StockNotes;

namespace StocksApi.Abstractions.Dal
{
    public interface IUsersDal
	{
        Task<List<User>> FindAllAsync();
        Task<User?> FindOneByIdAsync(string id);
        Task CreateAsync(User user);
        Task RemoveAsync(string id);
        Task<User?> FindOneByEmailAsync(string email);
        Task UpdatePasswordAsync(string email, string updatedPassword);
        Task AddNotificationAsync(string userEmail, StockNotification notification);
        Task AddWatchListNameAsync(string userEmail, string listName);
        Task RemoveWatchListNameAsync(string email, string listName);
        Task NotifyUserAsync(string userEmail, string notificationId);
        Task RemoveNotificationAsync(string userId, string notificationId);
        Task AddUserStockNoteAsync(string userEmail, string stockSymbol, UserStockNote userStockNote);
        Task RemoveUserStockNoteAsync(string userEmail, string stockSymbol, string noteId);
        Task UpdateUserStockNoteAsync(UserStockNoteUpdateRequest noteUpdateRequest);
    }
}
