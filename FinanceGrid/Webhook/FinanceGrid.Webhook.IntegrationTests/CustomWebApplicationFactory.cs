using FinanceGrid.Webhook.Api;
using FinanceGrid.Webhook.Domain.Entities;
using FinanceGrid.Webhook.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.Webhook.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;

    public const string SeedUserEmail = "test@example.com";
    public const string SeedEventType = "stock.updated";
    public const string SeedTargetUrl = "https://test.example.com/webhook";
    public const string SeedSecret = "test-secret-123";
    public static readonly Guid SeedSubscriptionId = Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            RemoveDbContextRegistrations(services);

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContextFactory<WebhookDbContext>(options =>
                options.UseSqlite(_connection));

            SeedDatabase(services);
        });
    }

    private static void RemoveDbContextRegistrations(IServiceCollection services)
    {
        var descriptorsToRemove = services
            .Where(d =>
                d.ServiceType == typeof(IDbContextFactory<WebhookDbContext>) ||
                d.ServiceType == typeof(DbContextOptions<WebhookDbContext>) ||
                (d.ServiceType.IsGenericType &&
                 d.ServiceType.GetGenericTypeDefinition() == typeof(IDbContextFactory<>) &&
                 d.ServiceType.GetGenericArguments()[0] == typeof(WebhookDbContext)))
            .ToList();

        foreach (var descriptor in descriptorsToRemove)
            services.Remove(descriptor);
    }

    private void SeedDatabase(IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WebhookDbContext>();

        db.Database.EnsureCreated();
        SeedSubscription(db);
        db.SaveChanges();
    }

    private static void SeedSubscription(WebhookDbContext db)
    {
        if (db.Subscriptions.Any()) return;

        db.Subscriptions.Add(new WebhookSubscription
        {
            Id = SeedSubscriptionId,
            UserEmail = SeedUserEmail,
            EventType = SeedEventType,
            TargetUrl = SeedTargetUrl,
            Secret = SeedSecret,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection?.Dispose();
    }
}
