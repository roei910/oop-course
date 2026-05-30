namespace FinanceGrid.Webhook.Domain.Entities;

public class WebhookDeliveryAttempt
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public string EventPayload { get; set; } = string.Empty;
    public int AttemptNumber { get; set; }
    public string Status { get; set; } = "Pending";
    public int? ResponseStatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;

    public WebhookSubscription? Subscription { get; set; }
}
