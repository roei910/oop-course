using FinanceGrid.Webhook.Domain.Entities;
using FinanceGrid.Webhook.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceGrid.Webhook.Infrastructure.Persistence.Repositories;

public class WebhookDeliveryRepository : IWebhookDeliveryRepository
{
    private readonly IDbContextFactory<WebhookDbContext> _contextFactory;

    public WebhookDeliveryRepository(IDbContextFactory<WebhookDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<WebhookDeliveryAttempt> RecordAttemptAsync(WebhookDeliveryAttempt attempt)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.DeliveryAttempts.Add(attempt);
        await context.SaveChangesAsync();
        return attempt;
    }

    public async Task<List<WebhookDeliveryAttempt>> GetAttemptsBySubscriptionAsync(Guid subscriptionId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.DeliveryAttempts
            .Where(a => a.SubscriptionId == subscriptionId)
            .OrderByDescending(a => a.AttemptedAt)
            .ToListAsync();
    }
}
