using Microsoft.EntityFrameworkCore;
using UsersAbstractions.Dal;
using UsersAbstractions.Models;
using UsersAbstractions.Models.Notifications;
using UsersAbstractions.Models.StockNotes;

namespace StocksApi.Providers.UsersSqlite.Dal
{
    public class UsersDal : IUsersDal
    {
        private readonly IDbContextFactory<UsersSqliteDbContext> _contextFactory;

        public UsersDal(IDbContextFactory<UsersSqliteDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<User>> FindAllAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Users.ToListAsync();
        }

        public async Task<User?> FindOneByIdAsync(string id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Users.FindAsync(id);
        }

        public async Task CreateAsync(User user)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task RemoveAsync(string id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var user = await context.Users.FindAsync(id);
            if (user is not null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<User?> FindOneByEmailAsync(string email)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdatePasswordAsync(string email, string updatedPassword)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is not null)
            {
                user.Password = updatedPassword;
                await context.SaveChangesAsync();
            }
        }

        public async Task AddNotificationAsync(string userEmail, StockNotification notification)
        {
            notification.Id = Guid.NewGuid().ToString();
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.StockNotifications.Add(notification);
            await context.SaveChangesAsync();
        }

        public async Task AddWatchListNameAsync(string userEmail, string listName)
        {
            var watchListName = new WatchListName
            {
                Id = Guid.NewGuid().ToString(),
                UserEmail = userEmail,
                Name = listName
            };
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.WatchListNames.Add(watchListName);
            await context.SaveChangesAsync();
        }

        public async Task RemoveWatchListNameAsync(string email, string listName)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var item = await context.WatchListNames
                .FirstOrDefaultAsync(w => w.UserEmail == email && w.Name == listName);
            if (item is not null)
            {
                context.WatchListNames.Remove(item);
                await context.SaveChangesAsync();
            }
        }

        public async Task NotifyUserAsync(string userEmail, string notificationId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var notification = await context.StockNotifications
                .FirstOrDefaultAsync(n => n.UserEmail == userEmail && n.Id == notificationId);
            if (notification is not null)
            {
                notification.ShouldBeNotified = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveNotificationAsync(string userId, string notificationId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var user = await context.Users.FindAsync(userId);
            if (user is null)
                return;

            var notification = await context.StockNotifications
                .FirstOrDefaultAsync(n => n.UserEmail == user.Email && n.Id == notificationId);
            if (notification is not null)
            {
                context.StockNotifications.Remove(notification);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddUserStockNoteAsync(string userEmail, string stockSymbol, UserStockNote userStockNote)
        {
            userStockNote.Id = Guid.NewGuid().ToString();
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.UserStockNotes.Add(userStockNote);
            context.Entry(userStockNote).Property("UserEmail").CurrentValue = userEmail;
            context.Entry(userStockNote).Property("StockSymbol").CurrentValue = stockSymbol;
            await context.SaveChangesAsync();
        }

        public async Task RemoveUserStockNoteAsync(string userEmail, string stockSymbol, string noteId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var note = await context.UserStockNotes
                .FirstOrDefaultAsync(n =>
                    n.Id == noteId &&
                    EF.Property<string>(n, "UserEmail") == userEmail &&
                    EF.Property<string>(n, "StockSymbol") == stockSymbol);
            if (note is not null)
            {
                context.UserStockNotes.Remove(note);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserStockNoteAsync(UserStockNoteUpdateRequest noteUpdateRequest)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var note = await context.UserStockNotes
                .FirstOrDefaultAsync(n =>
                    n.Id == noteUpdateRequest.Id &&
                    EF.Property<string>(n, "UserEmail") == noteUpdateRequest.UserEmail &&
                    EF.Property<string>(n, "StockSymbol") == noteUpdateRequest.StockSymbol);
            if (note is not null)
            {
                note.Note = noteUpdateRequest.Note;
                note.LastUpdateTime = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
        }
    }
}
