using FinanceGrid.Webhook.Infrastructure.Persistence;
using FinanceGrid.Webhook.Processing;
using Microsoft.EntityFrameworkCore;

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

using (var scope = host.Services.CreateScope())
{
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<WebhookDbContext>>();
    await using var context = await contextFactory.CreateDbContextAsync();
    await context.Database.EnsureCreatedAsync();
}

host.Run();
