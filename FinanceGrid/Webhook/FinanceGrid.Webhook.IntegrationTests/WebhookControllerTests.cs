using FinanceGrid.Webhook.Api.Controllers;
using FinanceGrid.Webhook.Domain.Entities;
using FinanceGrid.Webhook.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinanceGrid.Webhook.IntegrationTests.Controllers;

public class WebhookControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public WebhookControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Subscribe_WithValidRequest_ReturnsSubscriptionWithId()
    {
        // Arrange
        var request = new SubscribeRequest
        {
            UserEmail = "newuser@example.com",
            EventType = "stock.price-changed",
            TargetUrl = "https://newuser.example.com/hook",
            Secret = "new-secret"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/webhook/subscribe", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var subscription = await response.Content.ReadFromJsonAsync<WebhookSubscription>(JsonOptions);
        Assert.NotNull(subscription);
        Assert.NotEqual(Guid.Empty, subscription.Id);
        Assert.Equal(request.UserEmail, subscription.UserEmail);
        Assert.Equal(request.EventType, subscription.EventType);
        Assert.Equal(request.TargetUrl, subscription.TargetUrl);
        Assert.Equal(request.Secret, subscription.Secret);
        Assert.True(subscription.IsActive);
    }

    [Fact]
    public async Task GetSubscriptions_ForExistingUser_ReturnsList()
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/webhook/subscriptions?userEmail={CustomWebApplicationFactory.SeedUserEmail}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var subscriptions = await response.Content.ReadFromJsonAsync<List<WebhookSubscription>>(JsonOptions);
        Assert.NotNull(subscriptions);
        Assert.NotEmpty(subscriptions);
        Assert.Contains(subscriptions, s => s.Id == CustomWebApplicationFactory.SeedSubscriptionId);
    }

    [Fact]
    public async Task GetSubscriptions_ForUnknownUser_ReturnsEmptyList()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/webhook/subscriptions?userEmail=ghost@example.com");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var subscriptions = await response.Content.ReadFromJsonAsync<List<WebhookSubscription>>(JsonOptions);
        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions);
    }

    [Fact]
    public async Task Unsubscribe_WithExistingId_ReturnsOk()
    {
        // Act
        var response = await _client.DeleteAsync(
            $"/api/webhook/unsubscribe/{CustomWebApplicationFactory.SeedSubscriptionId}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify the subscription is now inactive
        var listResponse = await _client.GetAsync(
            $"/api/webhook/subscriptions?userEmail={CustomWebApplicationFactory.SeedUserEmail}");
        var subs = await listResponse.Content.ReadFromJsonAsync<List<WebhookSubscription>>(JsonOptions);
        Assert.NotNull(subs);
        var sub = subs.FirstOrDefault(s => s.Id == CustomWebApplicationFactory.SeedSubscriptionId);
        Assert.NotNull(sub);
        Assert.False(sub.IsActive);
    }

    [Fact]
    public async Task Unsubscribe_WithUnknownId_ReturnsOk()
    {
        // Act
        var unknownId = Guid.NewGuid();
        var response = await _client.DeleteAsync($"/api/webhook/unsubscribe/{unknownId}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Subscribe_TwiceWithSameEvent_CreatesMultipleSubscriptions()
    {
        // Arrange
        var first = new SubscribeRequest
        {
            UserEmail = "multi@example.com",
            EventType = "stock.updated",
            TargetUrl = "https://multi.example.com/hook1",
            Secret = "secret-1"
        };
        var second = new SubscribeRequest
        {
            UserEmail = "multi@example.com",
            EventType = "stock.updated",
            TargetUrl = "https://multi.example.com/hook2",
            Secret = "secret-2"
        };

        // Act
        var firstResponse = await _client.PostAsJsonAsync("/api/webhook/subscribe", first);
        var secondResponse = await _client.PostAsJsonAsync("/api/webhook/subscribe", second);

        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();

        var firstSub = await firstResponse.Content.ReadFromJsonAsync<WebhookSubscription>(JsonOptions);
        var secondSub = await secondResponse.Content.ReadFromJsonAsync<WebhookSubscription>(JsonOptions);

        Assert.NotNull(firstSub);
        Assert.NotNull(secondSub);
        Assert.NotEqual(firstSub.Id, secondSub.Id);

        var listResponse = await _client.GetAsync("/api/webhook/subscriptions?userEmail=multi@example.com");
        var subs = await listResponse.Content.ReadFromJsonAsync<List<WebhookSubscription>>(JsonOptions);
        Assert.NotNull(subs);
        Assert.Equal(2, subs.Count);
    }

    [Fact]
    public async Task GetSubscriptions_FilteredByUser_ReturnsOnlyMatching()
    {
        // Arrange: Subscribe another user
        var otherUser = new SubscribeRequest
        {
            UserEmail = "otheruser@example.com",
            EventType = "user.registered",
            TargetUrl = "https://other.example.com/hook",
            Secret = null
        };
        await _client.PostAsJsonAsync("/api/webhook/subscribe", otherUser);

        // Act
        var response = await _client.GetAsync("/api/webhook/subscriptions?userEmail=otheruser@example.com");

        // Assert
        response.EnsureSuccessStatusCode();
        var subs = await response.Content.ReadFromJsonAsync<List<WebhookSubscription>>(JsonOptions);
        Assert.NotNull(subs);
        Assert.All(subs, s => Assert.Equal("otheruser@example.com", s.UserEmail));
        Assert.DoesNotContain(subs, s => s.UserEmail == CustomWebApplicationFactory.SeedUserEmail);
    }
}
