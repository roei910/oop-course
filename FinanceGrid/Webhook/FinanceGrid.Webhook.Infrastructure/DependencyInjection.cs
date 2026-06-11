using FinanceGrid.Webhook.Application.Interfaces;
using FinanceGrid.Webhook.Application.Services;
using FinanceGrid.Webhook.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.Webhook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWebhookServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddWebhookDatabase(configuration);

        services.AddScoped<IWebhookService, WebhookService>();

        return services;
    }
}
