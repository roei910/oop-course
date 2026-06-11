using FinanceGrid.SystemTests.Dtos;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinanceGrid.SystemTests;

[Collection("DockerCompose")]
[Trait("Category", "System")]
public class WebhookFlowTests
{
    private readonly DockerComposeFixture _fixture;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public WebhookFlowTests(DockerComposeFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Subscribe_Then_GetSubscriptions_Flow_Succeeds()
    {
        RequireServices();
        using var http = new HttpClient { BaseAddress = new Uri(DockerComposeFixture.WebhookBaseUrl) };

        var email = $"sys-{Guid.NewGuid():N}@example.com";
        var request = new SubscribeRequest
        {
            UserEmail = email,
            EventType = "stock.updated",
            TargetUrl = "https://test.example.com/hook",
            Secret = "test-secret"
        };

        var subResp = await http.PostAsJsonAsync("/api/webhook/subscribe", request);
        Assert.Equal(HttpStatusCode.OK, subResp.StatusCode);

        var sub = await subResp.Content.ReadFromJsonAsync<WebhookSubscription>(JsonOptions);
        Assert.NotNull(sub);
        Assert.NotEqual(Guid.Empty, sub.Id);

        var getResp = await http.GetAsync($"/api/webhook/subscriptions?userEmail={email}");
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);

        var subs = await getResp.Content.ReadFromJsonAsync<List<WebhookSubscription>>(JsonOptions);
        Assert.NotNull(subs);
        Assert.Contains(subs, s => s.Id == sub.Id);
    }

    [Fact]
    public async Task Unsubscribe_Flow_Succeeds()
    {
        RequireServices();
        using var http = new HttpClient { BaseAddress = new Uri(DockerComposeFixture.WebhookBaseUrl) };

        var email = $"sys-{Guid.NewGuid():N}@example.com";
        var request = new SubscribeRequest
        {
            UserEmail = email,
            EventType = "user.registered",
            TargetUrl = "https://test.example.com/unsub",
            Secret = null
        };

        var subResp = await http.PostAsJsonAsync("/api/webhook/subscribe", request);
        var sub = await subResp.Content.ReadFromJsonAsync<WebhookSubscription>(JsonOptions);
        Assert.NotNull(sub);

        var unsubResp = await http.DeleteAsync($"/api/webhook/unsubscribe/{sub.Id}");
        Assert.Equal(HttpStatusCode.OK, unsubResp.StatusCode);
    }

    [Fact]
    public async Task GetSubscriptions_ForUnknownUser_ReturnsEmptyList()
    {
        RequireServices();
        using var http = new HttpClient { BaseAddress = new Uri(DockerComposeFixture.WebhookBaseUrl) };

        var resp = await http.GetAsync($"/api/webhook/subscriptions?userEmail=ghost-{Guid.NewGuid():N}@example.com");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var subs = await resp.Content.ReadFromJsonAsync<List<WebhookSubscription>>(JsonOptions);
        Assert.NotNull(subs);
        Assert.Empty(subs);
    }

    private void RequireServices()
    {
        if (!_fixture.ServicesAreRunning)
        {
            Assert.Fail("Docker Compose services are not running. " +
                "Set MANAGE_DOCKER_COMPOSE=true or start the stack manually.");
        }
    }
}
