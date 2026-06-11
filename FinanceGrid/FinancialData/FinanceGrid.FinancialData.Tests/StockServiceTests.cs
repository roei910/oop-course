using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Application.Services;
using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Tests;

public class StockServiceTests
{
    private readonly Mock<IStockRepository> _stockRepoMock = new();
    private readonly Mock<IFinanceStrategy> _financeStrategyMock = new();
    private readonly Mock<ILogger<StockService>> _loggerMock = new();
    private readonly StockService _service;

    public StockServiceTests()
    {
        _service = new StockService(
            _stockRepoMock.Object,
            _financeStrategyMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllStocksAsync_DelegatesToRepository()
    {
        var stocks = new List<Stock> { new() { Symbol = "AAPL" } };
        _stockRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(stocks);

        var result = await _service.GetAllStocksAsync();

        Assert.Single(result);
        Assert.Equal("AAPL", result[0].Symbol);
    }

    [Fact]
    public async Task GetStockBySymbolAsync_FoundInRepo_ReturnsExisting()
    {
        var existing = new Stock { Symbol = "AAPL" };
        _stockRepoMock.Setup(r => r.GetStockBySymbolAsync("AAPL")).ReturnsAsync(existing);

        var result = await _service.GetStockBySymbolAsync("aapl");

        Assert.NotNull(result);
        Assert.Equal("AAPL", result.Symbol);
        _financeStrategyMock.Verify(f => f.GetStockAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetStockBySymbolAsync_NotFound_CallsFinanceStrategy()
    {
        _stockRepoMock.Setup(r => r.GetStockBySymbolAsync("AAPL")).ReturnsAsync((Stock?)null);
        _financeStrategyMock.Setup(f => f.GetStockAsync("AAPL")).ReturnsAsync(new Stock { Symbol = "AAPL" });

        var result = await _service.GetStockBySymbolAsync("aapl");

        Assert.NotNull(result);
        Assert.Equal("AAPL", result.Symbol);
        _financeStrategyMock.Verify(f => f.GetStockAsync("AAPL"), Times.Once);
    }

    [Fact]
    public async Task GetStockBySymbolAsync_FinanceStrategyFails_ReturnsNull()
    {
        _stockRepoMock.Setup(r => r.GetStockBySymbolAsync("AAPL")).ReturnsAsync((Stock?)null);
        _financeStrategyMock.Setup(f => f.GetStockAsync("AAPL")).ThrowsAsync(new Exception("API error"));

        var result = await _service.GetStockBySymbolAsync("aapl");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetStocksBySymbolsAsync_AllFound_ReturnsWithoutApi()
    {
        var stocks = new List<Stock> { new() { Symbol = "AAPL" }, new() { Symbol = "MSFT" } };
        _stockRepoMock.Setup(r => r.GetStocksBySymbolAsync(It.IsAny<string[]>())).ReturnsAsync(stocks);

        var result = await _service.GetStocksBySymbolsAsync(["aapl", "msft"]);

        Assert.Equal(2, result.Count);
        _financeStrategyMock.Verify(f => f.GetStocksAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetStocksBySymbolsAsync_PartialFound_FetchesMissing()
    {
        var found = new List<Stock> { new() { Symbol = "AAPL" } };
        var fetched = new List<Stock> { new() { Symbol = "MSFT" } };
        _stockRepoMock.Setup(r => r.GetStocksBySymbolAsync(It.IsAny<string[]>())).ReturnsAsync(found);
        _financeStrategyMock.Setup(f => f.GetStocksAsync(It.IsAny<string>())).ReturnsAsync(fetched);

        var result = await _service.GetStocksBySymbolsAsync(["aapl", "msft"]);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, s => s.Symbol == "AAPL");
        Assert.Contains(result, s => s.Symbol == "MSFT");
    }

    [Fact]
    public async Task GetStocksBySymbolsAsync_NoneFound_FetchesAll()
    {
        _stockRepoMock.Setup(r => r.GetStocksBySymbolAsync(It.IsAny<string[]>())).ReturnsAsync([]);
        _financeStrategyMock.Setup(f => f.GetStocksAsync(It.IsAny<string>()))
            .ReturnsAsync([new Stock { Symbol = "AAPL" }]);

        var result = await _service.GetStocksBySymbolsAsync(["aapl"]);

        Assert.Single(result);
        Assert.Equal("AAPL", result[0].Symbol);
    }

    [Fact]
    public async Task GetStocksBySymbolsAsync_EmptyArray_ReturnsEmpty()
    {
        _stockRepoMock.Setup(r => r.GetStocksBySymbolAsync(It.IsAny<string[]>())).ReturnsAsync([]);

        var result = await _service.GetStocksBySymbolsAsync([]);

        Assert.Empty(result);
        _financeStrategyMock.Verify(f => f.GetStocksAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ForceUpdateAllStocksAsync_GetsAllAndUpdates()
    {
        var stocks = new List<Stock>
        {
            new() { Symbol = "AAPL" },
            new() { Symbol = "MSFT" }
        };
        _stockRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(stocks);
        _financeStrategyMock.Setup(f => f.GetStocksAsync("AAPL,MSFT"))
            .ReturnsAsync(stocks);

        await _service.ForceUpdateAllStocksAsync();

        _stockRepoMock.Verify(r => r.UpdateStocksAsync(
            It.Is<List<Stock>>(s => s.Count == 2)), Times.Once);
    }

    [Fact]
    public async Task UpdateStocksBySymbolAsync_FetchesFromApiAndUpdatesRepo()
    {
        var stocks = new List<Stock> { new() { Symbol = "AAPL" } };
        _financeStrategyMock.Setup(f => f.GetStocksAsync("AAPL")).ReturnsAsync(stocks);

        await _service.UpdateStocksBySymbolAsync(["AAPL"]);

        _financeStrategyMock.Verify(f => f.GetStocksAsync("AAPL"), Times.Once);
        _stockRepoMock.Verify(r => r.UpdateStocksAsync(
            It.Is<List<Stock>>(s => s.Count == 1 && s[0].Symbol == "AAPL")), Times.Once);
    }

    [Fact]
    public async Task UpdateStocksBySymbolAsync_ApiReturnsEmpty_DoesNotUpdateRepo()
    {
        _financeStrategyMock.Setup(f => f.GetStocksAsync("AAPL"))
            .ReturnsAsync(new List<Stock>());

        await _service.UpdateStocksBySymbolAsync(["AAPL"]);

        _stockRepoMock.Verify(r => r.UpdateStocksAsync(
            It.IsAny<List<Stock>>()), Times.Never);
    }

    [Fact]
    public async Task UpdateStocksBySymbolAsync_ApiThrows_DoesNotThrow()
    {
        _financeStrategyMock.Setup(f => f.GetStocksAsync("AAPL"))
            .ThrowsAsync(new Exception("API error"));

        await _service.UpdateStocksBySymbolAsync(["AAPL"]);

        _stockRepoMock.Verify(r => r.UpdateStocksAsync(
            It.IsAny<List<Stock>>()), Times.Never);
    }

    [Fact]
    public async Task UpdateStocksAnalysisAsync_FetchesFromApiAndUpdatesRepo()
    {
        var analyses = new List<StockAnalysis> { new() { Symbol = "AAPL" } };
        _financeStrategyMock.Setup(f => f.GetStocksAnalysisAsync(It.IsAny<string[]>()))
            .ReturnsAsync(analyses);

        await _service.UpdateStocksAnalysisAsync(["AAPL"]);

        _financeStrategyMock.Verify(f => f.GetStocksAnalysisAsync(
            It.Is<string[]>(s => s[0] == "AAPL")), Times.Once);
        _stockRepoMock.Verify(r => r.UpdateStocksAnalysisAsync(
            It.Is<List<StockAnalysis>>(a => a.Count == 1 && a[0].Symbol == "AAPL")), Times.Once);
    }

    [Fact]
    public async Task UpdateStocksAnalysisAsync_ApiReturnsEmpty_DoesNotUpdateRepo()
    {
        _financeStrategyMock.Setup(f => f.GetStocksAnalysisAsync(It.IsAny<string[]>()))
            .ReturnsAsync(new List<StockAnalysis>());

        await _service.UpdateStocksAnalysisAsync(["AAPL"]);

        _stockRepoMock.Verify(r => r.UpdateStocksAnalysisAsync(
            It.IsAny<List<StockAnalysis>>()), Times.Never);
    }
}
