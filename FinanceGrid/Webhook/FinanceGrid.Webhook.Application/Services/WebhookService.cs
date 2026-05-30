using FinanceGrid.Webhook.Application.Interfaces;
using FinanceGrid.Webhook.Domain.Entities;
using FinanceGrid.Webhook.Domain.Interfaces;

namespace FinanceGrid.Webhook.Application.Services;

public class WebhookService : IWebhookService
{
    private readonly IWebhookSubscriptionRepository _subscriptionRepo;

    public WebhookService(IWebhookSubscriptionRepository subscriptionRepo)
    {
        _subscriptionRepo = subscriptionRepo;
    }

    public async Task<WebhookSubscription> SubscribeAsync(string userEmail, string eventType,
        string targetUrl, string? secret)
    {
        var subscription = new WebhookSubscription
        {
            Id = Guid.NewGuid(),
            UserEmail = userEmail,
            EventType = eventType,
            TargetUrl = targetUrl,
            Secret = secret,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return await _subscriptionRepo.CreateAsync(subscription);
    }

    public async Task UnsubscribeAsync(Guid id)
    {
        await _subscriptionRepo.DeactivateAsync(id);
    }

    public async Task<List<WebhookSubscription>> GetUserSubscriptionsAsync(string userEmail)
    {
        return await _subscriptionRepo.GetByUserEmailAsync(userEmail);
    }
}
