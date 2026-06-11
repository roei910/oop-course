using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinanceGrid.Users.IntegrationTests.Controllers;

public class UserStockWatchControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UserStockWatchControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetByEmail_WithListName_ReturnsFilteredWatches()
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/userStockWatch?email={CustomWebApplicationFactory.SeedUserEmail}&listName=Default");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var watches = await response.Content.ReadFromJsonAsync<List<UserStockWatch>>(JsonOptions);
        Assert.NotNull(watches);
        Assert.NotEmpty(watches);
        Assert.All(watches, w => Assert.Equal("Default", w.ListName));
        Assert.All(watches, w => Assert.Equal(CustomWebApplicationFactory.SeedUserEmail, w.UserEmail));
    }

    [Fact]
    public async Task GetByEmail_WithoutListName_ReturnsAllWatches()
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/userStockWatch?email={CustomWebApplicationFactory.SeedUserEmail}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var watches = await response.Content.ReadFromJsonAsync<List<UserStockWatch>>(JsonOptions);
        Assert.NotNull(watches);
        Assert.NotEmpty(watches);
        Assert.All(watches, w => Assert.Equal(CustomWebApplicationFactory.SeedUserEmail, w.UserEmail));
    }

    [Fact]
    public async Task GetByEmail_ForUserWithNoWatches_ReturnsEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/userStockWatch?email=ghost@example.com");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var watches = await response.Content.ReadFromJsonAsync<List<UserStockWatch>>(JsonOptions);
        Assert.NotNull(watches);
        Assert.Empty(watches);
    }
}
