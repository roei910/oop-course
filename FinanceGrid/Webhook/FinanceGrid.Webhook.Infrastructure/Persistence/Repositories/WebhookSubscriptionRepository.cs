using FinanceGrid.Webhook.Domain.Entities;
using FinanceGrid.Webhook.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceGrid.Webhook.Infrastructure.Persistence.Repositories;

public class WebhookSubscriptionRepository : IWebhookSubscriptionRepository
{
    private readonly IDbContextFactory<WebhookDbContext> _contextFactory;

    public WebhookSubscriptionRepository(IDbContextFactory<WebhookDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<WebhookSubscription> CreateAsync(WebhookSubscription subscription)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Subscriptions.Add(subscription);
        await context.SaveChangesAsync();
        return subscription;
    }

    public async Task DeactivateAsync(Guid id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var sub = await context.Subscriptions.FindAsync(id);
        if (sub is not null)
        {
            sub.IsActive = false;
            await context.SaveChangesAsync();
        }
    }

    public async Task<List<WebhookSubscription>> GetByUserEmailAsync(string userEmail)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Subscriptions
            .Where(s => s.UserEmail == userEmail)
            .ToListAsync();
    }

    public async Task<List<WebhookSubscription>> GetActiveByEventTypeAsync(string eventType)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Subscriptions
            .Where(s => s.EventType == eventType && s.IsActive)
            .ToListAsync();
    }

    public async Task<WebhookSubscription?> GetByIdAsync(Guid id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Subscriptions.FindAsync(id);
    }
}
