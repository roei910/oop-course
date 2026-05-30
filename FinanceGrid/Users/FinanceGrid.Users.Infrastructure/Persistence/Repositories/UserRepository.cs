using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FinanceGrid.Shared;
using System.Security.Cryptography;
using System.Text;

namespace FinanceGrid.Users.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<UsersDbContext> _contextFactory;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        IDbContextFactory<UsersDbContext> contextFactory,
        ILogger<UserRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<List<User>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetAsync(string email)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(string firstName, string lastName, string email, string password)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = HashPassword(password)
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ConnectUserAsync(string email, string password)
    {
        var user = await GetAsync(email);
        if (user?.Password is null) return false;
        return VerifyPassword(password, user.Password);
    }

    public async Task RemoveUserAsync(string id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var user = await context.Users.FindAsync(id);
        if (user is not null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdatePasswordAsync(string email, string password)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is not null)
        {
            user.Password = HashPassword(password);
            await context.SaveChangesAsync();
        }
    }

    public Task AddNotificationAsync(StockNotification notification)
    {
        throw new NotSupportedException("Notifications managed by Webhook service");
    }

    public Task RemoveNotificationAsync(string id, string notificationId)
    {
        throw new NotSupportedException("Notifications managed by Webhook service");
    }

    public Task ShowNotificationAsync(StockNotification notification)
    {
        throw new NotSupportedException("Notifications managed by Webhook service");
    }

    public Task<UserStockNote> AddStockNoteAsync(string userEmail, string stockSymbol, string note)
    {
        throw new NotSupportedException("Stock notes managed by dedicated service");
    }

    public Task RemoveStockNoteAsync(string userEmail, string stockSymbol, string noteId)
    {
        throw new NotSupportedException("Stock notes managed by dedicated service");
    }

    public Task UpdateStockNoteAsync(string noteId, string userEmail, string stockSymbol, string note)
    {
        throw new NotSupportedException("Stock notes managed by dedicated service");
    }

    private static string HashPassword(string password)
    {
        var salt = AppVariables.PASSWORD_SALT;
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password + salt));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        return HashPassword(password) == hashedPassword;
    }
}
