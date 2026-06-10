using FinanceGrid.Shared.Database;
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
        var dbConfig = configuration.GetDatabaseConfiguration("Webhook");

        services.AddDbContextFactory<WebhookDbContext>(options =>
        {
            if (dbConfig.IsPostgreSQL)
                options.UseNpgsql(dbConfig.ConnectionString);
            else
                options.UseSqlite(dbConfig.ConnectionString);
        });

        services.AddSingleton<IWebhookSubscriptionRepository, WebhookSubscriptionRepository>();
        services.AddSingleton<IWebhookDeliveryRepository, WebhookDeliveryRepository>();

        return services;
    }
}
