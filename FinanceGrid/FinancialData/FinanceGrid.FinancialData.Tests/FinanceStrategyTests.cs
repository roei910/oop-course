using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Tests;

public class FinanceStrategyTests
{
    [Fact]
    public async Task GetStockAsync_UsesRoundRobin()
    {
        var api1 = new Mock<IYahooFinance>();
        var api2 = new Mock<IYahooFinance>();
        api1.Setup(a => a.GetStockAsync("AAPL")).ReturnsAsync(new Stock { Symbol = "AAPL", Price = 150 });
        api2.Setup(a => a.GetStockAsync("MSFT")).ReturnsAsync(new Stock { Symbol = "MSFT", Price = 300 });
        var logger = new Mock<ILogger<FinanceStrategy>>();

        var strategy = new FinanceStrategy(new[] { api1.Object, api2.Object }, Array.Empty<IStockAnalysisApi>(), logger.Object);

        var result1 = await strategy.GetStockAsync("AAPL");
        var result2 = await strategy.GetStockAsync("MSFT");

        Assert.Equal("AAPL", result1!.Symbol);
        Assert.Equal("MSFT", result2!.Symbol);
        api1.Verify(a => a.GetStockAsync("AAPL"), Times.Once);
        api2.Verify(a => a.GetStockAsync("MSFT"), Times.Once);
    }

    [Fact]
    public async Task GetStocksAsync_WithBulkApi_UsesBulk()
    {
        var bulkApi = new Mock<IYahooFinanceBulk>();
        bulkApi.Setup(b => b.GetStocksAsync(It.IsAny<string[]>()))
            .ReturnsAsync([new Stock { Symbol = "AAPL" }, new Stock { Symbol = "MSFT" }]);
        var logger = new Mock<ILogger<FinanceStrategy>>();

        var strategy = new FinanceStrategy(
            new[] { bulkApi.As<IYahooFinance>().Object },
            Array.Empty<IStockAnalysisApi>(),
            logger.Object);

        var result = await strategy.GetStocksAsync("AAPL,MSFT");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetStocksAsync_NoBulkApi_FallsBackToSingle()
    {
        var api = new Mock<IYahooFinance>();
        api.Setup(a => a.GetStockAsync(It.IsAny<string>())).ReturnsAsync(new Stock { Symbol = "FALLBACK" });
        var logger = new Mock<ILogger<FinanceStrategy>>();

        var strategy = new FinanceStrategy(
            new[] { api.Object },
            Array.Empty<IStockAnalysisApi>(),
            logger.Object);

        var result = await strategy.GetStocksAsync("AAPL,MSFT");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        api.Verify(a => a.GetStockAsync(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetStocksAsync_EmptySymbols_ReturnsEmpty()
    {
        var api = new Mock<IYahooFinance>();
        var logger = new Mock<ILogger<FinanceStrategy>>();
        var strategy = new FinanceStrategy(new[] { api.Object }, Array.Empty<IStockAnalysisApi>(), logger.Object);

        var result = await strategy.GetStocksAsync("");

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task FindStockAsync_DelegatesToApi()
    {
        var api = new Mock<IYahooFinance>();
        var results = new List<StockSearchResult> { new() { Symbol = "AAPL", Name = "Apple", ExchDisp = "NASDAQ", TypeDisp = "Equity" } };
        api.Setup(a => a.FindStockAsync("apple")).ReturnsAsync(results);
        var logger = new Mock<ILogger<FinanceStrategy>>();
        var strategy = new FinanceStrategy(new[] { api.Object }, Array.Empty<IStockAnalysisApi>(), logger.Object);

        var findResult = await strategy.FindStockAsync("apple");

        Assert.Single(findResult);
        Assert.Equal("AAPL", findResult[0].Symbol);
    }

    [Fact]
    public async Task GetStockAnalysisAsync_DelegatesToAnalysisApi()
    {
        var analysisApi = new Mock<IStockAnalysisApi>();
        var analysis = new StockAnalysis();
        analysisApi.Setup(a => a.GetStockAnalysisAsync("AAPL")).ReturnsAsync(analysis);
        var logger = new Mock<ILogger<FinanceStrategy>>();
        var strategy = new FinanceStrategy(
            Array.Empty<IYahooFinance>(),
            new[] { analysisApi.Object },
            logger.Object);

        var result = await strategy.GetStockAnalysisAsync("AAPL");

        Assert.NotNull(result);
        analysisApi.Verify(a => a.GetStockAnalysisAsync("AAPL"), Times.Once);
    }

    [Fact]
    public async Task GetStocksAnalysisAsync_DelegatesToAnalysisApi()
    {
        var analysisApi = new Mock<IStockAnalysisApi>();
        var analyses = new List<StockAnalysis> { new() };
        analysisApi.Setup(a => a.GetStocksAnalysisAsync(It.IsAny<string[]>())).ReturnsAsync(analyses);
        var logger = new Mock<ILogger<FinanceStrategy>>();
        var strategy = new FinanceStrategy(
            Array.Empty<IYahooFinance>(),
            new[] { analysisApi.Object },
            logger.Object);

        var result = await strategy.GetStocksAnalysisAsync(["AAPL", "MSFT"]);

        Assert.Single(result);
        analysisApi.Verify(a => a.GetStocksAnalysisAsync(It.IsAny<string[]>()), Times.Once);
    }
}
