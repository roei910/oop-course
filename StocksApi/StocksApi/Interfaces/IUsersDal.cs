using Library.Models;
using Library.Models.Users;

namespace StocksApi.Interfaces
{
	public interface IUsersDal
	{
        Task<List<User>> FindAllAsync();
        Task<User?> FindOneByIdAsync(string id);
        Task CreateAsync(User user);
        Task UpdateAsync(string id, User updatedUser);
        Task RemoveAsync(string id);
        Task<User?> FindOneByEmailAsync(string email);
        Task UpdatePasswordAsync(string email, string updatedPassword);
        Task AddNotificationAsync(string userEmail, StockNotification notification);
        Task AddStockListAsync(string userEmail, string listName);
        Task RemoveStockListAsync(string email, string listName);
        Task NotifyUserAsync(string userEmail, string notificationId);
        Task RemoveNotificationAsync(string userId, string notificationId);
        Task AddUserStockNoteAsync(string userEmail, string stockSymbol, UserStockNote userStockNote);
        Task RemoveUserStockNoteAsync(string userEmail, string stockSymbol, string noteId);
    }
}