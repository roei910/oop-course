using FinanceGrid.Webhook.Domain.Entities;

namespace FinanceGrid.Webhook.Application.Interfaces;

public interface IWebhookService
{
    Task<WebhookSubscription> SubscribeAsync(string userEmail, string eventType, string targetUrl, string? secret);
    Task UnsubscribeAsync(Guid id);
    Task<List<WebhookSubscription>> GetUserSubscriptionsAsync(string userEmail);
}
