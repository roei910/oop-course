using UsersAbstractions.Models;
using UsersAbstractions.Models.Users;
using UsersAbstractions.Models.Notifications;
using UsersAbstractions.Models.StockNotes;

namespace UsersAbstractions.Repositories
{
    public interface IUserRepository
    {
        Task AddNotificationAsync(StockNotification notification);
        Task AddUserAsync(UserDetails userDetails);
        Task<bool> ConnectUserAsync(UserCredentials user);
        Task RemoveNotificationAsync(string id, string notificationId);
        Task<List<User>> GetAllAsync();
        Task<User?> GetAsync(string email);
        Task RemoveUserAsync(string id);
        Task ShowNotificationAsync(StockNotification notification);
        Task UpdatePasswordAsync(PasswordUpdateRequest passwordUpdateRequest);
        Task<UserStockNote> AddStockNoteAsync(UserStockNoteRequest userStockNoteRequest);
        Task RemoveStockNoteAsync(string userEmail, string stockSymbol, string noteId);
        Task UpdateStockNoteAsync(UserStockNoteUpdateRequest noteUpdateRequest);
    }
}
