using Microsoft.EntityFrameworkCore;
using UsersAbstractions.Dal;
using UsersAbstractions.Models.Shares;

namespace StocksApi.Providers.UsersSqlite.Dal
{
    public class UserStockWatchesDal : IUserStockWatchesDal
    {
        private readonly IDbContextFactory<UsersSqliteDbContext> _contextFactory;

        public UserStockWatchesDal(IDbContextFactory<UsersSqliteDbContext> contextFactory)
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
                .FirstOrDefaultAsync(w =>
                    w.UserEmail == email &&
                    w.ListName == listName &&
                    w.StockSymbol == stockSymbol);
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

        public async Task AddShareToWatchAsync(string email, string listName, string stockSymbol, string shareId, Share share)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var watch = await context.UserStockWatches
                .FirstOrDefaultAsync(w =>
                    w.UserEmail == email &&
                    w.ListName == listName &&
                    w.StockSymbol == stockSymbol);
            if (watch is not null)
            {
                context.Shares.Add(share);
                context.Entry(share).Property("UserStockWatchId").CurrentValue = watch.Id;
                context.Entry(share).Property("PurchaseGuid").CurrentValue = shareId;
                await context.SaveChangesAsync();
            }
        }

        public async Task UpsertWatchAndAddShareAsync(string email, string listName, string stockSymbol, string shareId, Share share)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var watch = await context.UserStockWatches
                .FirstOrDefaultAsync(w =>
                    w.UserEmail == email &&
                    w.ListName == listName &&
                    w.StockSymbol == stockSymbol);

            if (watch is null)
            {
                watch = new UserStockWatch
                {
                    Id = Guid.NewGuid().ToString(),
                    UserEmail = email,
                    ListName = listName,
                    StockSymbol = stockSymbol,
                    Note = null,
                    PurchaseGuidToShares = new Dictionary<string, Share>()
                };
                context.UserStockWatches.Add(watch);
                await context.SaveChangesAsync();
            }

            context.Shares.Add(share);
            context.Entry(share).Property("UserStockWatchId").CurrentValue = watch.Id;
            context.Entry(share).Property("PurchaseGuid").CurrentValue = shareId;
            await context.SaveChangesAsync();
        }

        public async Task RemoveShareFromWatchAsync(string email, string listName, string stockSymbol, string shareId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var share = await context.Shares
                .FirstOrDefaultAsync(s =>
                    EF.Property<string>(s, "PurchaseGuid") == shareId);
            if (share is not null)
            {
                context.Shares.Remove(share);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateWatchNoteAsync(string email, string listName, string stockSymbol, string? note)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var watch = await context.UserStockWatches
                .FirstOrDefaultAsync(w =>
                    w.UserEmail == email &&
                    w.ListName == listName &&
                    w.StockSymbol == stockSymbol);
            if (watch is not null)
            {
                watch.Note = note;
                await context.SaveChangesAsync();
            }
        }
    }
}
