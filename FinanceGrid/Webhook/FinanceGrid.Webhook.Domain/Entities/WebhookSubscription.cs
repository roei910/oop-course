namespace FinanceGrid.Webhook.Domain.Entities;

public class WebhookSubscription
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string TargetUrl { get; set; } = string.Empty;
    public string? Secret { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
