using FinanceGrid.Shared;
using FinanceGrid.Users.Api;
using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;

namespace FinanceGrid.Users.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;

    public const string SeedUserEmail = "test@example.com";
    public const string SeedUserPassword = "Password123!";
    public const string SeedUserFirstName = "Test";
    public const string SeedUserLastName = "User";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            RemoveDbContextRegistrations(services);

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContextFactory<UsersDbContext>(options =>
                options.UseSqlite(_connection));

            SeedDatabase(services);
        });
    }

    private static void RemoveDbContextRegistrations(IServiceCollection services)
    {
        var descriptorsToRemove = services
            .Where(d =>
                d.ServiceType == typeof(IDbContextFactory<UsersDbContext>) ||
                d.ServiceType == typeof(DbContextOptions<UsersDbContext>) ||
                (d.ServiceType.IsGenericType &&
                 d.ServiceType.GetGenericTypeDefinition() == typeof(IDbContextFactory<>) &&
                 d.ServiceType.GetGenericArguments()[0] == typeof(UsersDbContext)))
            .ToList();

        foreach (var descriptor in descriptorsToRemove)
            services.Remove(descriptor);
    }

    private void SeedDatabase(IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        db.Database.EnsureCreated();
        SeedUser(db);
        SeedUserList(db);
        SeedWatchingStocks(db);
        db.SaveChanges();
    }

    private static void SeedUser(UsersDbContext db)
    {
        if (db.Users.Any()) return;

        db.Users.Add(new User
        {
            FirstName = SeedUserFirstName,
            LastName = SeedUserLastName,
            Email = SeedUserEmail,
            Password = HashPassword(SeedUserPassword)
        });
    }

    private static void SeedUserList(UsersDbContext db)
    {
        if (db.UserStockWatches.Any()) return;

        db.UserStockWatches.Add(new UserStockWatch
        {
            UserEmail = SeedUserEmail,
            ListName = "Default",
            StockSymbol = "AAPL",
            PurchaseGuidToShares = new Dictionary<string, Share>
            {
                ["seed-share-1"] = new Share
                {
                    Id = "seed-share-1",
                    PurchasingPrice = 145.00,
                    PurchaseDate = DateTime.UtcNow.AddDays(-30),
                    Amount = 10
                }
            }
        });
    }

    private static void SeedWatchingStocks(UsersDbContext db)
    {
        // Add another watch for filter testing
        if (db.UserStockWatches.Count() >= 2) return;

        var existing = db.UserStockWatches.FirstOrDefault();
        if (existing is null) return;

        db.UserStockWatches.Add(new UserStockWatch
        {
            UserEmail = SeedUserEmail,
            ListName = "Tech",
            StockSymbol = "MSFT",
            PurchaseGuidToShares = new Dictionary<string, Share>()
        });
    }

    private static string HashPassword(string password)
    {
        var salt = AppVariables.PASSWORD_SALT;
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password + salt));
        return Convert.ToBase64String(bytes);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection?.Dispose();
    }
}
