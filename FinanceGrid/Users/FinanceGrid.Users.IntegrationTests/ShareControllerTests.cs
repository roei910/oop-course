using FinanceGrid.Users.Api.Controllers;
using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinanceGrid.Users.IntegrationTests.Controllers;

public class ShareControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ShareControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddShare_WithExistingUser_ReturnsOkWithShare()
    {
        // Arrange
        var request = new SharePurchaseRequest
        {
            UserEmail = CustomWebApplicationFactory.SeedUserEmail,
            StockSymbol = "AAPL",
            PurchaseDate = DateTime.UtcNow.AddDays(-7),
            PurchasingPrice = 150.00,
            Amount = 5,
            ListName = "Default"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/share", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var share = await response.Content.ReadFromJsonAsync<Share>(JsonOptions);
        Assert.NotNull(share);
        Assert.Equal(150.00, share.PurchasingPrice);
        Assert.Equal(5, share.Amount);
    }

    [Fact]
    public async Task AddShare_WithMissingUser_ReturnsNotFound()
    {
        // Arrange
        var request = new SharePurchaseRequest
        {
            UserEmail = "ghost@example.com",
            StockSymbol = "AAPL",
            PurchaseDate = DateTime.UtcNow,
            PurchasingPrice = 100.00,
            Amount = 1,
            ListName = "Default"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/share", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RemoveShare_WithExistingWatch_ReturnsOk()
    {
        // Arrange
        var removeRequest = new ShareSaleRequest
        {
            UserEmail = CustomWebApplicationFactory.SeedUserEmail,
            ListName = "Default",
            StockSymbol = "AAPL",
            SharePurchaseGuid = "seed-share-1"
        };

        // Act
        var response = await _client.DeleteAsync("/api/share") is not null
            ? await SendDeleteWithBody("/api/share", removeRequest)
            : default!;

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RemoveShare_WithMissingUser_ReturnsNotFound()
    {
        // Arrange
        var removeRequest = new ShareSaleRequest
        {
            UserEmail = "ghost@example.com",
            ListName = "Default",
            StockSymbol = "AAPL",
            SharePurchaseGuid = "any-guid"
        };

        // Act
        var response = await SendDeleteWithBody("/api/share", removeRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddUserList_WithExistingUser_ReturnsOk()
    {
        // Arrange
        var request = new StockListRequest
        {
            UserEmail = CustomWebApplicationFactory.SeedUserEmail,
            ListName = $"List-{Guid.NewGuid()}"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/share/list", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddUserList_WithEmptyListName_ReturnsBadRequest()
    {
        // Arrange
        var request = new StockListRequest
        {
            UserEmail = CustomWebApplicationFactory.SeedUserEmail,
            ListName = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/share/list", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddUserList_WithMissingUser_ReturnsNotFound()
    {
        // Arrange
        var request = new StockListRequest
        {
            UserEmail = "ghost@example.com",
            ListName = "NewList"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/share/list", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RemoveUserList_WithExistingList_ReturnsOk()
    {
        // Arrange: Create a list first
        var listName = $"TempList-{Guid.NewGuid()}";
        var createRequest = new StockListRequest
        {
            UserEmail = CustomWebApplicationFactory.SeedUserEmail,
            ListName = listName
        };
        await _client.PostAsJsonAsync("/api/share/list", createRequest);

        // Act
        var response = await SendDeleteWithBody("/api/share/list", createRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RemoveUserList_WithMissingUser_ReturnsNotFound()
    {
        // Arrange
        var request = new StockListRequest
        {
            UserEmail = "ghost@example.com",
            ListName = "AnyList"
        };

        // Act
        var response = await SendDeleteWithBody("/api/share/list", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddWatchingStock_WithExistingUser_ReturnsOk()
    {
        // Arrange
        var listName = $"WatchList-{Guid.NewGuid()}";
        // First add the list
        await _client.PostAsJsonAsync("/api/share/list", new StockListRequest
        {
            UserEmail = CustomWebApplicationFactory.SeedUserEmail,
            ListName = listName
        });

        var request = new WatchingStockRequest
        {
            Email = CustomWebApplicationFactory.SeedUserEmail,
            ListName = listName,
            StockSymbol = "TSLA"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/share/watching-stock", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddWatchingStock_WithMissingUser_ReturnsNotFound()
    {
        // Arrange
        var request = new WatchingStockRequest
        {
            Email = "ghost@example.com",
            ListName = "Default",
            StockSymbol = "TSLA"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/share/watching-stock", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RemoveWatchingStock_ReturnsOk()
    {
        // Arrange: Add a watch first
        var symbol = $"REM{Guid.NewGuid().ToString()[..4]}";
        var listName = $"WatchList-{Guid.NewGuid()}";
        await _client.PostAsJsonAsync("/api/share/list", new StockListRequest
        {
            UserEmail = CustomWebApplicationFactory.SeedUserEmail,
            ListName = listName
        });
        await _client.PostAsJsonAsync("/api/share/watching-stock", new WatchingStockRequest
        {
            Email = CustomWebApplicationFactory.SeedUserEmail,
            ListName = listName,
            StockSymbol = symbol
        });

        var request = new WatchingStockRequest
        {
            Email = CustomWebApplicationFactory.SeedUserEmail,
            ListName = listName,
            StockSymbol = symbol
        };

        // Act
        var response = await SendDeleteWithBody("/api/share/watching-stock", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateWatchingStockNote_ReturnsOk()
    {
        // Arrange
        var request = new WatchingStockRequest
        {
            Email = CustomWebApplicationFactory.SeedUserEmail,
            ListName = "Default",
            StockSymbol = "AAPL",
            Note = "Test note from integration test"
        };

        // Act
        var requestMessage = new HttpRequestMessage(HttpMethod.Patch, "/api/share/watching-stock-note")
        {
            Content = JsonContent.Create(request)
        };
        var response = await _client.SendAsync(requestMessage);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private async Task<HttpResponseMessage> SendDeleteWithBody(string url, object body)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url)
        {
            Content = JsonContent.Create(body)
        };
        return await _client.SendAsync(requestMessage);
    }
}
