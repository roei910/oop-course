using FinanceGrid.Users.Domain.Entities;

namespace FinanceGrid.Users.Application.Interfaces;

public interface IShareService
{
    Task<Share?> AddShareAsync(string userEmail, string listName, string stockSymbol, double purchasingPrice, DateTime purchaseDate, double amount);
    Task RemoveShareAsync(string userEmail, string listName, string stockSymbol, string sharePurchaseGuid);
    Task AddUserListAsync(string userEmail, string listName);
    Task RemoveUserListAsync(string userEmail, string listName);
    Task AddWatchingStockAsync(string email, string listName, string stockSymbol);
    Task RemoveWatchingStockAsync(string email, string listName, string stockSymbol);
    Task UpdateWatchingStockNoteAsync(string email, string listName, string stockSymbol, string? note);
}
