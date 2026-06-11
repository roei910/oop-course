using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Services;
using FinanceGrid.Shared;

namespace FinanceGrid.FinancialData.Tests;

public class StockMarketTimeServiceTests
{
    private readonly StockMarketTimeService _service = new();

    private DateTime Monday1300 => CreateUtc(DayOfWeek.Monday, 13, 0);
    private DateTime Monday1330 => CreateUtc(DayOfWeek.Monday, 13, 30);
    private DateTime Monday1500 => CreateUtc(DayOfWeek.Monday, 15, 0);
    private DateTime Monday1900 => CreateUtc(DayOfWeek.Monday, 19, 0);
    private DateTime Tuesday1400 => CreateUtc(DayOfWeek.Tuesday, 14, 0);
    private DateTime Friday1400 => CreateUtc(DayOfWeek.Friday, 14, 0);
    private DateTime Saturday1200 => CreateUtc(DayOfWeek.Saturday, 12, 0);
    private DateTime Sunday1200 => CreateUtc(DayOfWeek.Sunday, 12, 0);

    [Fact]
    public void IsMarketOpen_BeforeOpenMinutes_ReturnsFalse()
    {
        Assert.False(_service.IsMarketOpen(Monday1300));
    }

    [Fact]
    public void IsMarketOpen_AtOpenExact_ReturnsTrue()
    {
        Assert.True(_service.IsMarketOpen(Monday1330));
    }

    [Fact]
    public void IsMarketOpen_DuringMarketHours_ReturnsTrue()
    {
        Assert.True(_service.IsMarketOpen(Monday1500));
    }

    [Fact]
    public void IsMarketOpen_AtCloseHour_ReturnsTrue()
    {
        Assert.True(_service.IsMarketOpen(Monday1900));
    }

    [Fact]
    public void IsMarketOpen_AfterCloseHour_ReturnsFalse()
    {
        var monday2000 = CreateUtc(DayOfWeek.Monday, 20, 0);
        Assert.False(_service.IsMarketOpen(monday2000));
    }

    [Fact]
    public void IsMarketOpen_Saturday_ReturnsFalse()
    {
        Assert.False(_service.IsMarketOpen(Saturday1200));
    }

    [Fact]
    public void IsMarketOpen_Sunday_ReturnsFalse()
    {
        Assert.False(_service.IsMarketOpen(Sunday1200));
    }

    [Fact]
    public void IsMarketOpen_Tuesday_ReturnsTrue()
    {
        Assert.True(_service.IsMarketOpen(Tuesday1400));
    }

    [Fact]
    public void IsMarketOpen_Friday_ReturnsTrue()
    {
        Assert.True(_service.IsMarketOpen(Friday1400));
    }

    [Fact]
    public void ShouldStockBeUpdated_StaleStockBeforeLastMarketClose_ReturnsTrue()
    {
        var stock = new Stock { Symbol = "AAPL", UpdatedTime = DateTime.UtcNow.AddDays(-4) };

        var result = _service.ShouldStockBeUpdated(stock);

        Assert.True(result);
    }

    [Fact]
    public void ShouldStockBeUpdated_RecentlyUpdatedStock_ReturnsFalse()
    {
        var stock = new Stock { Symbol = "AAPL", UpdatedTime = DateTime.UtcNow.AddMinutes(-30) };

        var result = _service.ShouldStockBeUpdated(stock);

        Assert.False(result);
    }

    [Fact]
    public void LastMarketCloseDateTime_ReturnsPastDate()
    {
        var result = _service.LastMarketCloseDateTime();

        Assert.True(result < DateTime.UtcNow);
        Assert.Equal(AppVariables.CLOSED_MARKET_HOURS, result.Hour);
        Assert.Equal(0, result.Minute);
    }

    private static DateTime CreateUtc(DayOfWeek day, int hour, int minute)
    {
        var now = DateTime.UtcNow;
        var diff = (int)day - (int)now.DayOfWeek;
        var date = now.AddDays(diff).Date;
        return DateTime.SpecifyKind(date.AddHours(hour).AddMinutes(minute), DateTimeKind.Utc);
    }
}
