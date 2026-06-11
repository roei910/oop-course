using FinanceGrid.Webhook.Domain.Entities;

namespace FinanceGrid.Webhook.Domain.Interfaces;

public interface IWebhookSubscriptionRepository
{
    Task<WebhookSubscription> CreateAsync(WebhookSubscription subscription);
    Task DeactivateAsync(Guid id);
    Task<List<WebhookSubscription>> GetByUserEmailAsync(string userEmail);
    Task<List<WebhookSubscription>> GetActiveByEventTypeAsync(string eventType);
    Task<WebhookSubscription?> GetByIdAsync(Guid id);
}
