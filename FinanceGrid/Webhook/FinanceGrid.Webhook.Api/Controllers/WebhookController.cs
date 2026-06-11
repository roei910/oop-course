using FinanceGrid.Webhook.Application.Interfaces;
using FinanceGrid.Webhook.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinanceGrid.Webhook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly IWebhookService _webhookService;

    public WebhookController(IWebhookService webhookService)
    {
        _webhookService = webhookService;
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult<WebhookSubscription>> Subscribe([FromBody] SubscribeRequest request)
    {
        var subscription = await _webhookService.SubscribeAsync(
            request.UserEmail, request.EventType, request.TargetUrl, request.Secret);
        return Ok(subscription);
    }

    [HttpDelete("unsubscribe/{id:guid}")]
    public async Task<IActionResult> Unsubscribe(Guid id)
    {
        await _webhookService.UnsubscribeAsync(id);
        return Ok();
    }

    [HttpGet("subscriptions")]
    public async Task<ActionResult<List<WebhookSubscription>>> GetSubscriptions(
        [FromQuery] string userEmail)
    {
        var subscriptions = await _webhookService.GetUserSubscriptionsAsync(userEmail);
        return Ok(subscriptions);
    }
}

public class SubscribeRequest
{
    public string UserEmail { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string TargetUrl { get; set; } = string.Empty;
    public string? Secret { get; set; }
}
