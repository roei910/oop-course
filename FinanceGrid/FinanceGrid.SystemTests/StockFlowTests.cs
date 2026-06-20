using FinanceGrid.SystemTests.Dtos;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinanceGrid.SystemTests;

[Collection("DockerCompose")]
[Trait("Category", "System")]
public class StockFlowTests
{
    private readonly DockerComposeFixture _fixture;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public StockFlowTests(DockerComposeFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAllStocks_ReturnsList()
    {
        RequireServices();
        using var http = new HttpClient { BaseAddress = new Uri(DockerComposeFixture.FinancialDataBaseUrl) };

        var resp = await http.GetAsync("/api/stock");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var stocks = await resp.Content.ReadFromJsonAsync<List<Stock>>(JsonOptions);
        Assert.NotNull(stocks);
        // Database starts empty in dev mode; just verify we get a list (possibly empty)
    }

    [Fact]
    public async Task GetStockBySymbol_ForUnknownSymbol_ReturnsNotFound()
    {
        RequireServices();
        using var http = new HttpClient { BaseAddress = new Uri(DockerComposeFixture.FinancialDataBaseUrl) };

        var resp = await http.GetAsync($"/api/stock/symbol/{Guid.NewGuid():N}");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Fact]
    public async Task GetMarketTrends_ReturnsList()
    {
        RequireServices();
        using var http = new HttpClient { BaseAddress = new Uri(DockerComposeFixture.FinancialDataBaseUrl) };

        var resp = await http.GetAsync("/api/stock/marketTrends");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var trends = await resp.Content.ReadFromJsonAsync<List<MarketTrend>>(JsonOptions);
        Assert.NotNull(trends);
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
