using FinanceGrid.FinancialData.Api;
using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.BackgroundServices;
using FinanceGrid.FinancialData.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FinanceGrid.FinancialData.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            RemoveDbContextRegistrations(services);
            RemoveHostedServices(services);

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContextFactory<FinancialDataDbContext>(options =>
                options.UseSqlite(_connection));

            SeedDatabase(services);
        });
    }

    private static void RemoveDbContextRegistrations(IServiceCollection services)
    {
        var descriptorsToRemove = services
            .Where(d =>
                d.ServiceType == typeof(IDbContextFactory<FinancialDataDbContext>) ||
                d.ServiceType == typeof(DbContextOptions<FinancialDataDbContext>) ||
                (d.ServiceType.IsGenericType &&
                 d.ServiceType.GetGenericTypeDefinition() == typeof(IDbContextFactory<>) &&
                 d.ServiceType.GetGenericArguments()[0] == typeof(FinancialDataDbContext)))
            .ToList();

        foreach (var descriptor in descriptorsToRemove)
            services.Remove(descriptor);
    }

    private static void RemoveHostedServices(IServiceCollection services)
    {
        var hostedServiceTypes = new[]
        {
            typeof(StocksAutomaticUpdater),
            typeof(StockAnalysisUpdater)
        };

        var descriptorsToRemove = services
            .Where(d => d.ServiceType == typeof(IHostedService) &&
                        d.ImplementationType != null &&
                        hostedServiceTypes.Contains(d.ImplementationType))
            .ToList();

        foreach (var descriptor in descriptorsToRemove)
            services.Remove(descriptor);
    }

    private void SeedDatabase(IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FinancialDataDbContext>();

        db.Database.EnsureCreated();
        SeedStocks(db);
        SeedMarketTrends(db);
        SeedSearchResults(db);
        db.SaveChanges();
    }

    private static void SeedStocks(FinancialDataDbContext db)
    {
        if (db.Stocks.Any()) return;

        var now = DateTime.UtcNow;
        db.Stocks.AddRange(
            new Stock
            {
                Id = "seed-stock-aapl",
                Symbol = "AAPL",
                Name = "Apple Inc.",
                Price = 150.00,
                RegularMarketPreviousClose = 148.50,
                RegularMarketOpen = 149.00,
                RegularMarketDayLow = 148.00,
                RegularMarketDayHigh = 151.00,
                RegularMarketDayRange = "148.00 - 151.00",
                RegularMarketChange = 1.50,
                RegularMarketChangePercent = 1.01,
                RegularMarketVolume = 50000000,
                FiftyDayAverage = 145.00,
                TwoHundredDayAverage = 140.00,
                FiftyTwoWeekRange = "120.00 - 180.00",
                FiftyTwoWeekLow = 120.00,
                FiftyTwoWeekHigh = 180.00,
                ForwardPE = 25.5,
                EpsCurrentYear = 6.05,
                EpsForward = 6.50,
                FullExchangeName = "NasdaqGS",
                AnalystRating = "Buy",
                UpdatedTime = now,
                Analysis = new StockAnalysis
                {
                    Symbol = "AAPL",
                    UpdatedTime = now,
                    TargetHighPrice = 200.00,
                    TargetLowPrice = 140.00,
                    TargetMeanPrice = 175.00,
                    TargetMedianPrice = 170.00
                }
            },
            new Stock
            {
                Id = "seed-stock-msft",
                Symbol = "MSFT",
                Name = "Microsoft Corporation",
                Price = 300.00,
                RegularMarketPreviousClose = 298.50,
                RegularMarketOpen = 299.00,
                RegularMarketDayLow = 297.00,
                RegularMarketDayHigh = 302.00,
                RegularMarketDayRange = "297.00 - 302.00",
                RegularMarketChange = 1.50,
                RegularMarketChangePercent = 0.50,
                RegularMarketVolume = 30000000,
                FiftyDayAverage = 295.00,
                TwoHundredDayAverage = 280.00,
                FiftyTwoWeekRange = "250.00 - 350.00",
                FiftyTwoWeekLow = 250.00,
                FiftyTwoWeekHigh = 350.00,
                ForwardPE = 30.0,
                EpsCurrentYear = 10.00,
                EpsForward = 11.50,
                FullExchangeName = "NasdaqGS",
                AnalystRating = "Buy",
                UpdatedTime = now,
                Analysis = new StockAnalysis
                {
                    Symbol = "MSFT",
                    UpdatedTime = now,
                    TargetHighPrice = 380.00,
                    TargetLowPrice = 280.00,
                    TargetMeanPrice = 330.00,
                    TargetMedianPrice = 320.00
                }
            },
            new Stock
            {
                Id = "seed-stock-googl",
                Symbol = "GOOGL",
                Name = "Alphabet Inc.",
                Price = 2800.00,
                RegularMarketPreviousClose = 2780.00,
                RegularMarketOpen = 2785.00,
                RegularMarketDayLow = 2770.00,
                RegularMarketDayHigh = 2810.00,
                RegularMarketDayRange = "2770.00 - 2810.00",
                RegularMarketChange = 20.00,
                RegularMarketChangePercent = 0.72,
                RegularMarketVolume = 1500000,
                FiftyDayAverage = 2750.00,
                TwoHundredDayAverage = 2600.00,
                FiftyTwoWeekRange = "2200.00 - 3000.00",
                FiftyTwoWeekLow = 2200.00,
                FiftyTwoWeekHigh = 3000.00,
                ForwardPE = 22.0,
                EpsCurrentYear = 105.00,
                EpsForward = 120.00,
                FullExchangeName = "NasdaqGS",
                AnalystRating = "Buy",
                UpdatedTime = now,
                Analysis = new StockAnalysis
                {
                    Symbol = "GOOGL",
                    UpdatedTime = now,
                    TargetHighPrice = 3200.00,
                    TargetLowPrice = 2500.00,
                    TargetMeanPrice = 2850.00,
                    TargetMedianPrice = 2800.00
                }
            }
        );
    }

    private static void SeedMarketTrends(FinancialDataDbContext db)
    {
        if (db.MarketTrends.Any()) return;

        db.MarketTrends.AddRange(
            new MarketTrend { Id = "seed-trend-bull", TrendName = "Bull Market", LastUpdatedTime = DateTime.UtcNow },
            new MarketTrend { Id = "seed-trend-bear", TrendName = "Bear Market", LastUpdatedTime = DateTime.UtcNow },
            new MarketTrend { Id = "seed-trend-sideways", TrendName = "Sideways Market", LastUpdatedTime = DateTime.UtcNow }
        );
    }

    private static void SeedSearchResults(FinancialDataDbContext db)
    {
        if (db.SearchResults.Any()) return;

        var appleSearch = new SearchResult
        {
            Id = "seed-search-apple",
            SearchTerm = "Apple",
            StockSearchResults = new List<StockSearchResult>
            {
                new() { Symbol = "AAPL", Name = "Apple Inc.", ExchDisp = "NASDAQ", TypeDisp = "Equity" }
            }
        };
        var microsoftSearch = new SearchResult
        {
            Id = "seed-search-microsoft",
            SearchTerm = "Microsoft",
            StockSearchResults = new List<StockSearchResult>
            {
                new() { Symbol = "MSFT", Name = "Microsoft Corporation", ExchDisp = "NASDAQ", TypeDisp = "Equity" }
            }
        };
        var googleSearch = new SearchResult
        {
            Id = "seed-search-google",
            SearchTerm = "Google",
            StockSearchResults = new List<StockSearchResult>
            {
                new() { Symbol = "GOOGL", Name = "Alphabet Inc.", ExchDisp = "NASDAQ", TypeDisp = "Equity" }
            }
        };

        db.SearchResults.AddRange(appleSearch, microsoftSearch, googleSearch);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection?.Dispose();
    }
}
