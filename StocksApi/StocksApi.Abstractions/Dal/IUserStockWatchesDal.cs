using StocksApi.Abstractions.Models.Shares;

namespace StocksApi.Abstractions.Dal
{
    public interface IUserStockWatchesDal
    {
        Task InsertWatchAsync(UserStockWatch watch);
        Task<List<UserStockWatch>> GetWatchesByEmailAsync(string email);
        Task<List<UserStockWatch>> GetWatchesByEmailAndListAsync(string email, string listName);
        Task DeleteWatchAsync(string email, string listName, string stockSymbol);
        Task DeleteWatchesByListAsync(string email, string listName);
        Task AddShareToWatchAsync(string email, string listName, string stockSymbol, string shareId, Share share);
        Task UpsertWatchAndAddShareAsync(string email, string listName, string stockSymbol, string shareId, Share share);
        Task RemoveShareFromWatchAsync(string email, string listName, string stockSymbol, string shareId);
        Task UpdateWatchNoteAsync(string email, string listName, string stockSymbol, string? note);
    }
}
