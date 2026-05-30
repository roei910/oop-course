using FinanceGrid.Users.Domain.Entities;

namespace FinanceGrid.Users.Domain.Interfaces;

public interface IUserRepository
{
    Task AddNotificationAsync(StockNotification notification);
    Task AddUserAsync(string firstName, string lastName, string email, string password);
    Task<bool> ConnectUserAsync(string email, string password);
    Task RemoveNotificationAsync(string id, string notificationId);
    Task<List<User>> GetAllAsync();
    Task<User?> GetAsync(string email);
    Task RemoveUserAsync(string id);
    Task ShowNotificationAsync(StockNotification notification);
    Task UpdatePasswordAsync(string email, string password);
    Task<UserStockNote> AddStockNoteAsync(string userEmail, string stockSymbol, string note);
    Task RemoveStockNoteAsync(string userEmail, string stockSymbol, string noteId);
    Task UpdateStockNoteAsync(string noteId, string userEmail, string stockSymbol, string note);
}
