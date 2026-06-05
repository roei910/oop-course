using FinanceGrid.Webhook.Application.Services;
using FinanceGrid.Webhook.Domain.Entities;
using FinanceGrid.Webhook.Domain.Interfaces;

namespace FinanceGrid.Webhook.Tests;

public class WebhookServiceTests
{
    private readonly Mock<IWebhookSubscriptionRepository> _repoMock = new();
    private readonly WebhookService _service;

    public WebhookServiceTests()
    {
        _service = new WebhookService(_repoMock.Object);
    }

    [Fact]
    public async Task SubscribeAsync_CreatesActiveSubscription()
    {
        _repoMock.Setup(r => r.CreateAsync(It.IsAny<WebhookSubscription>()))
            .ReturnsAsync((WebhookSubscription s) => s);

        var result = await _service.SubscribeAsync("test@test.com", "stock.price_updated",
            "https://example.com/hook", "secret123");

        Assert.Equal("test@test.com", result.UserEmail);
        Assert.Equal("stock.price_updated", result.EventType);
        Assert.Equal("https://example.com/hook", result.TargetUrl);
        Assert.Equal("secret123", result.Secret);
        Assert.True(result.IsActive);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task SubscribeAsync_WithoutSecret_SetsNull()
    {
        _repoMock.Setup(r => r.CreateAsync(It.IsAny<WebhookSubscription>()))
            .ReturnsAsync((WebhookSubscription s) => s);

        var result = await _service.SubscribeAsync("test@test.com", "stock.price_updated",
            "https://example.com/hook", null);

        Assert.Null(result.Secret);
    }

    [Fact]
    public async Task UnsubscribeAsync_DelegatesToRepository()
    {
        var id = Guid.NewGuid();

        await _service.UnsubscribeAsync(id);

        _repoMock.Verify(r => r.DeactivateAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetUserSubscriptionsAsync_DelegatesToRepository()
    {
        var subs = new List<WebhookSubscription>
        {
            new() { UserEmail = "test@test.com", EventType = "stock.price_updated" }
        };
        _repoMock.Setup(r => r.GetByUserEmailAsync("test@test.com")).ReturnsAsync(subs);

        var result = await _service.GetUserSubscriptionsAsync("test@test.com");

        Assert.Single(result);
        Assert.Equal("test@test.com", result[0].UserEmail);
    }

    [Fact]
    public async Task GetUserSubscriptionsAsync_NoSubscriptions_ReturnsEmpty()
    {
        _repoMock.Setup(r => r.GetByUserEmailAsync("unknown@test.com")).ReturnsAsync([]);

        var result = await _service.GetUserSubscriptionsAsync("unknown@test.com");

        Assert.Empty(result);
    }
}
