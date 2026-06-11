using FinanceGrid.Shared.Database;
using FinanceGrid.Webhook.Infrastructure.Persistence;
using FinanceGrid.Webhook.Processing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults(typeof(Program).Assembly);

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
    var dbConfig = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<WebhookDbContext>>();
    await using var context = await contextFactory.CreateDbContextAsync();
    if (dbConfig.IsPostgreSQL)
    {
        await context.Database.MigrateAsync();
    }
    else
    {
        await context.Database.EnsureCreatedAsync();
    }
}

host.Run();
