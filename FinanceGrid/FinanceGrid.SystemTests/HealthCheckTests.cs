using System.Net;
using Xunit;

namespace FinanceGrid.SystemTests;

[Collection("DockerCompose")]
[Trait("Category", "System")]
public class HealthCheckTests
{
    private readonly DockerComposeFixture _fixture;

    public HealthCheckTests(DockerComposeFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task FinancialDataService_IsHealthy()
    {
        RequireServices();
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        var response = await http.GetAsync($"{DockerComposeFixture.FinancialDataBaseUrl}/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UsersService_IsHealthy()
    {
        RequireServices();
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        var response = await http.GetAsync($"{DockerComposeFixture.UsersBaseUrl}/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task WebhookService_IsHealthy()
    {
        RequireServices();
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        var response = await http.GetAsync($"{DockerComposeFixture.WebhookBaseUrl}/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private void RequireServices()
    {
        if (!_fixture.ServicesAreRunning)
        {
            Assert.Fail("Docker Compose services are not running. " +
                "Set MANAGE_DOCKER_COMPOSE=true (with Docker available) " +
                "or start the stack manually (docker compose up -d).");
        }
    }
}
