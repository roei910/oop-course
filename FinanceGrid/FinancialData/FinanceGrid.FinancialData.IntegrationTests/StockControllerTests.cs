using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using System.Collections.Generic;
using Xunit;

namespace FinanceGrid.FinancialData.IntegrationTests.Controllers;

public class StockControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public StockControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllStocks_ReturnsOk_WithListOfStocks()
    {
        // Act
        var response = await _client.GetAsync("/api/stock");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var stocks = await response.Content.ReadFromJsonAsync<List<Stock>>();
        Assert.NotNull(stocks);
        Assert.NotEmpty(stocks);
        Assert.Equal(3, stocks.Count); // We seeded 3 stocks
    }

    [Fact]
    public async Task GetStockBySymbol_WithValidSymbol_ReturnsOk_WithStock()
    {
        // Act
        var response = await _client.GetAsync("/api/stock/symbol/AAPL");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var stock = await response.Content.ReadFromJsonAsync<Stock>();
        Assert.NotNull(stock);
        Assert.Equal("AAPL", stock.Symbol);
        Assert.Equal("Apple Inc.", stock.Name);
        Assert.Equal(150.00, stock.Price);
    }

    [Fact]
    public async Task GetStockBySymbol_WithInvalidSymbol_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/stock/symbol/INVALID");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetStocksBySymbols_WithValidSymbols_ReturnsOk_WithStocks()
    {
        // Arrange
        var symbols = new[] { "AAPL", "MSFT" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/stock/symbol/bulk", symbols);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var stocks = await response.Content.ReadFromJsonAsync<List<Stock>>();
        Assert.NotNull(stocks);
        Assert.Equal(2, stocks.Count);
        Assert.Contains(stocks, s => s.Symbol == "AAPL");
        Assert.Contains(stocks, s => s.Symbol == "MSFT");
    }

    [Fact]
    public async Task GetStocksBySymbols_WithEmptyArray_ReturnsOk_WithEmptyList()
    {
        // Arrange
        var symbols = new string[0];

        // Act
        var response = await _client.PostAsJsonAsync("/api/stock/symbol/bulk", symbols);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var stocks = await response.Content.ReadFromJsonAsync<List<Stock>>();
        Assert.NotNull(stocks);
        Assert.Empty(stocks);
    }

    [Fact]
    public async Task FindSymbolByTerm_WithValidSearchTerm_ReturnsOk_WithResults()
    {
        // Act
        var response = await _client.GetAsync("/api/stock/find/Apple");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var results = await response.Content.ReadFromJsonAsync<List<StockSearchResult>>();
        Assert.NotNull(results);
        Assert.NotEmpty(results);
        // Should find Apple Inc. based on our seeded data
    }

    [Fact]
    public async Task FindSymbolByTerm_WithInvalidSearchTerm_ReturnsOk_WithEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/stock/find/NonExistentCompany");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var results = await response.Content.ReadFromJsonAsync<List<StockSearchResult>>();
        Assert.NotNull(results);
        Assert.Empty(results);
    }

    [Fact]
    public async Task GetMarketTrends_ReturnsOk_WithListOfTrends()
    {
        // Act
        var response = await _client.GetAsync("/api/stock/marketTrends");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var trends = await response.Content.ReadFromJsonAsync<List<MarketTrend>>();
        Assert.NotNull(trends);
        Assert.NotEmpty(trends);
        Assert.Equal(3, trends.Count); // We seeded 3 trends
    }

    [Fact]
    public async Task ForceUpdateAllStocks_ReturnsInternalServerError_DueToKnownRepositoryBug()
    {
        // Act
        var response = await _client.PostAsync("/api/stock/force-update-all-stocks", null);

        // Assert
        // StockRepository.UpdateStocksBySymbolAsync throws NotSupportedException,
        // causing the controller to return 500. This test documents the known bug
        // and will be updated when the repository method is implemented.
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}