using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceGrid.Users.Infrastructure.Persistence.Repositories;

public class UserStockWatchRepository : IUserStockWatchRepository
{
    private readonly IDbContextFactory<UsersDbContext> _contextFactory;

    public UserStockWatchRepository(IDbContextFactory<UsersDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task InsertWatchAsync(UserStockWatch watch)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.UserStockWatches.Add(watch);
        await context.SaveChangesAsync();
    }

    public async Task<List<UserStockWatch>> GetWatchesByEmailAsync(string email)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.UserStockWatches
            .Where(w => w.UserEmail == email)
            .ToListAsync();
    }

    public async Task<List<UserStockWatch>> GetWatchesByEmailAndListAsync(string email, string listName)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.UserStockWatches
            .Where(w => w.UserEmail == email && w.ListName == listName)
            .ToListAsync();
    }

    public async Task DeleteWatchAsync(string email, string listName, string stockSymbol)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var watch = await context.UserStockWatches
            .FirstOrDefaultAsync(w => w.UserEmail == email && w.ListName == listName && w.StockSymbol == stockSymbol);
        if (watch is not null)
        {
            context.UserStockWatches.Remove(watch);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteWatchesByListAsync(string email, string listName)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var watches = await context.UserStockWatches
            .Where(w => w.UserEmail == email && w.ListName == listName)
            .ToListAsync();
        context.UserStockWatches.RemoveRange(watches);
        await context.SaveChangesAsync();
    }

    public async Task UpsertWatchAndAddShareAsync(string email, string listName,
        string stockSymbol, string shareId, Share share)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var watch = await context.UserStockWatches
            .FirstOrDefaultAsync(w => w.UserEmail == email && w.ListName == listName && w.StockSymbol == stockSymbol);

        if (watch is null)
        {
            watch = new UserStockWatch
            {
                UserEmail = email,
                ListName = listName,
                StockSymbol = stockSymbol,
                PurchaseGuidToShares = new Dictionary<string, Share> { { shareId, share } }
            };
            context.UserStockWatches.Add(watch);
        }
        else
        {
            watch.PurchaseGuidToShares[shareId] = share;
        }

        await context.SaveChangesAsync();
    }

    public async Task RemoveShareFromWatchAsync(string email, string listName,
        string stockSymbol, string sharePurchaseGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var watch = await context.UserStockWatches
            .FirstOrDefaultAsync(w => w.UserEmail == email && w.ListName == listName && w.StockSymbol == stockSymbol);
        if (watch is not null)
        {
            watch.PurchaseGuidToShares.Remove(sharePurchaseGuid);
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateWatchNoteAsync(string email, string listName,
        string stockSymbol, string? note)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var watch = await context.UserStockWatches
            .FirstOrDefaultAsync(w => w.UserEmail == email && w.ListName == listName && w.StockSymbol == stockSymbol);
        if (watch is not null)
        {
            watch.Note = note;
            await context.SaveChangesAsync();
        }
    }
}
