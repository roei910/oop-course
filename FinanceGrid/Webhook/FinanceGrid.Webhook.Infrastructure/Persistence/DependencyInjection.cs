using FinanceGrid.Webhook.Domain.Interfaces;
using FinanceGrid.Webhook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.Webhook.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddWebhookPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var provider = configuration.GetValue<string>("DatabaseProvider") ?? "SQLite";
        var connectionString = configuration.GetConnectionString("Webhook")
            ?? "Data Source=Webhook.db";

        services.AddDbContextFactory<WebhookDbContext>(options =>
        {
            if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
                options.UseNpgsql(connectionString);
            else
                options.UseSqlite(connectionString);
        });

        services.AddSingleton<IWebhookSubscriptionRepository, WebhookSubscriptionRepository>();
        services.AddSingleton<IWebhookDeliveryRepository, WebhookDeliveryRepository>();

        return services;
    }
}
