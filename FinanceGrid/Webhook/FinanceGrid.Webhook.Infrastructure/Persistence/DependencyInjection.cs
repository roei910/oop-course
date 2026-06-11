using FinanceGrid.Shared.Database;
using FinanceGrid.Webhook.Domain.Interfaces;
using FinanceGrid.Webhook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FinanceGrid.Webhook.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddWebhookPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration, "Webhook");

        services.AddDbContextFactory<WebhookDbContext>((sp, options) =>
        {
            var dbConfig = sp.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;
            switch (dbConfig.Provider.ToLower())
            {
                case "postgresql":
                    options.UseNpgsql(dbConfig.ConnectionString);
                    break;
                case "sqlite":
                default:
                    options.UseSqlite(dbConfig.ConnectionString);
                    break;
            }
        });

        services.AddSingleton<IWebhookSubscriptionRepository, WebhookSubscriptionRepository>();
        services.AddSingleton<IWebhookDeliveryRepository, WebhookDeliveryRepository>();

        return services;
    }
}
