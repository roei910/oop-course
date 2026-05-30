using FinanceGrid.Webhook.Application.Interfaces;
using FinanceGrid.Webhook.Application.Services;
using FinanceGrid.Webhook.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.Webhook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWebhookInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddWebhookPersistence(configuration);

        services.AddScoped<IWebhookService, WebhookService>();

        return services;
    }
}
