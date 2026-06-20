using FinanceGrid.Users.Domain.Entities;

namespace FinanceGrid.Users.Domain.Interfaces;

public interface IUserStockWatchRepository
{
    Task InsertWatchAsync(UserStockWatch watch);
    Task<List<UserStockWatch>> GetWatchesByEmailAsync(string email);
    Task<List<UserStockWatch>> GetWatchesByEmailAndListAsync(string email, string listName);
    Task DeleteWatchAsync(string email, string listName, string stockSymbol);
    Task DeleteWatchesByListAsync(string email, string listName);
    Task UpsertWatchAndAddShareAsync(string email, string listName, string stockSymbol, string shareId, Share share);
    Task RemoveShareFromWatchAsync(string email, string listName, string stockSymbol, string sharePurchaseGuid);
    Task UpdateWatchNoteAsync(string email, string listName, string stockSymbol, string? note);
}
