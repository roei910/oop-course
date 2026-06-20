using FinanceGrid.Users.Application.Interfaces;
using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.Domain.Interfaces;

namespace FinanceGrid.Users.Application.Services;

public class ShareService : IShareService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserStockWatchRepository _watchRepository;

    public ShareService(IUserRepository userRepository, IUserStockWatchRepository watchRepository)
    {
        _userRepository = userRepository;
        _watchRepository = watchRepository;
    }

    public async Task<Share?> AddShareAsync(string userEmail, string listName, string stockSymbol,
        double purchasingPrice, DateTime purchaseDate, double amount)
    {
        var user = await _userRepository.GetAsync(userEmail);
        if (user is null) return null;

        var share = new Share
        {
            Id = Guid.NewGuid().ToString(),
            PurchasingPrice = purchasingPrice,
            PurchaseDate = purchaseDate,
            Amount = amount
        };

        await _watchRepository.UpsertWatchAndAddShareAsync(
            userEmail, listName, stockSymbol.ToUpper(), share.Id, share);

        if (!user.WatchListNames.Contains(listName))
        {
            user.WatchListNames.Add(listName);
        }

        return share;
    }

    public async Task RemoveShareAsync(string userEmail, string listName,
        string stockSymbol, string sharePurchaseGuid)
    {
        await _watchRepository.RemoveShareFromWatchAsync(
            userEmail, listName, stockSymbol.ToUpper(), sharePurchaseGuid);
    }

    public async Task AddUserListAsync(string userEmail, string listName)
    {
        var user = await _userRepository.GetAsync(userEmail);
        if (user is null) return;

        user.WatchListNames.Add(listName);
    }

    public async Task RemoveUserListAsync(string userEmail, string listName)
    {
        await _watchRepository.DeleteWatchesByListAsync(userEmail, listName);
        var user = await _userRepository.GetAsync(userEmail);
        user?.WatchListNames.Remove(listName);
    }

    public async Task AddWatchingStockAsync(string email, string listName, string stockSymbol)
    {
        var watch = new UserStockWatch
        {
            UserEmail = email,
            ListName = listName,
            StockSymbol = stockSymbol.ToUpper(),
            PurchaseGuidToShares = []
        };
        await _watchRepository.InsertWatchAsync(watch);
    }

    public async Task RemoveWatchingStockAsync(string email, string listName, string stockSymbol)
    {
        await _watchRepository.DeleteWatchAsync(email, listName, stockSymbol.ToUpper());
    }

    public async Task UpdateWatchingStockNoteAsync(string email, string listName,
        string stockSymbol, string? note)
    {
        await _watchRepository.UpdateWatchNoteAsync(email, listName, stockSymbol.ToUpper(), note);
    }
}
