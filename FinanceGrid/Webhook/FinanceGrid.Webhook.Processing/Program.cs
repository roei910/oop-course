using FinanceGrid.Webhook.Infrastructure.Persistence;
using FinanceGrid.Webhook.Processing;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddWebhookPersistence(builder.Configuration);

builder.Services.AddHttpClient("webhook-delivery", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHostedService<WebhookDeliveryWorker>();

builder.Services.AddSingleton<WebhookEventPublisher>();

var host = builder.Build();
host.Run();
