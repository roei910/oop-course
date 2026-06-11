using FinanceGrid.Webhook.Domain.Entities;

namespace FinanceGrid.Webhook.Domain.Interfaces;

public interface IWebhookDeliveryRepository
{
    Task<WebhookDeliveryAttempt> RecordAttemptAsync(WebhookDeliveryAttempt attempt);
    Task<List<WebhookDeliveryAttempt>> GetAttemptsBySubscriptionAsync(Guid subscriptionId);
}
