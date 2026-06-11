using FinanceGrid.Persistence;
using FinanceGrid.Webhook.Domain.Interfaces;
using FinanceGrid.Webhook.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.Webhook.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddWebhookDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<WebhookDbContext>(configuration, "Webhook");

        services.AddSingleton<IWebhookSubscriptionRepository, WebhookSubscriptionRepository>();
        services.AddSingleton<IWebhookDeliveryRepository, WebhookDeliveryRepository>();

        return services;
    }
}
