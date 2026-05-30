using FinanceGrid.Shared;
using FinanceGrid.Webhook.Domain.Entities;
using FinanceGrid.Webhook.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FinanceGrid.Webhook.Processing;

public class WebhookDeliveryWorker : BackgroundService
{
    private readonly IWebhookSubscriptionRepository _subscriptionRepo;
    private readonly IWebhookDeliveryRepository _deliveryRepo;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<WebhookDeliveryWorker> _logger;

    private const int MaxRetries = 3;
    private const int BaseDelaySeconds = 10;

    public WebhookDeliveryWorker(
        IWebhookSubscriptionRepository subscriptionRepo,
        IWebhookDeliveryRepository deliveryRepo,
        IHttpClientFactory httpClientFactory,
        ILogger<WebhookDeliveryWorker> logger)
    {
        _subscriptionRepo = subscriptionRepo;
        _deliveryRepo = deliveryRepo;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("WebhookDeliveryWorker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingEventsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in webhook processing loop");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private async Task ProcessPendingEventsAsync(CancellationToken ct)
    {
        var subscriptions = await _subscriptionRepo.GetActiveByEventTypeAsync("stock.price_updated");

        foreach (var sub in subscriptions)
        {
            var payload = JsonSerializer.Serialize(new
            {
                event_type = sub.EventType,
                timestamp = DateTime.UtcNow,
                data = new { message = "Stock price updated" }
            });

            await DeliverWithRetryAsync(sub, payload, ct);
        }
    }

    private async Task DeliverWithRetryAsync(WebhookSubscription sub, string payload, CancellationToken ct)
    {
        for (int attempt = 1; attempt <= MaxRetries; attempt++)
        {
            var record = new WebhookDeliveryAttempt
            {
                Id = Guid.NewGuid(),
                SubscriptionId = sub.Id,
                EventPayload = payload,
                AttemptNumber = attempt,
                AttemptedAt = DateTime.UtcNow
            };

            try
            {
                using var client = _httpClientFactory.CreateClient("webhook-delivery");
                var request = new HttpRequestMessage(HttpMethod.Post, sub.TargetUrl)
                {
                    Content = new StringContent(payload, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(sub.Secret))
                {
                    var signature = ComputeHmacSignature(payload, sub.Secret);
                    request.Headers.Add("X-Webhook-Signature", signature);
                }

                var response = await client.SendAsync(request, ct);
                record.ResponseStatusCode = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    record.Status = "Delivered";
                    await _deliveryRepo.RecordAttemptAsync(record);
                    _logger.LogInformation("Webhook delivered to {Url}", sub.TargetUrl);
                    return;
                }

                record.Status = "Failed";
                record.ErrorMessage = $"HTTP {response.StatusCode}";
                await _deliveryRepo.RecordAttemptAsync(record);
            }
            catch (Exception ex)
            {
                record.Status = "Failed";
                record.ErrorMessage = ex.Message;
                await _deliveryRepo.RecordAttemptAsync(record);
                _logger.LogWarning(ex, "Webhook delivery attempt {Attempt}/{MaxRetries} failed for {Url}",
                    attempt, MaxRetries, sub.TargetUrl);
            }

            if (attempt < MaxRetries)
                await Task.Delay(TimeSpan.FromSeconds(BaseDelaySeconds * attempt), ct);
        }
    }

    private static string ComputeHmacSignature(string payload, string secret)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);
        var hash = HMACSHA256.HashData(keyBytes, payloadBytes);
        return Convert.ToBase64String(hash);
    }
}

public class WebhookEventPublisher
{
    public event Func<string, Task>? OnEventPublished;

    public async Task PublishEventAsync(string eventType)
    {
        if (OnEventPublished is not null)
            await OnEventPublished.Invoke(eventType);
    }
}
