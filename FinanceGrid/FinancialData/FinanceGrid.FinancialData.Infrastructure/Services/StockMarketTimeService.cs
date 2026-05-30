using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;
using FinanceGrid.Shared;

namespace FinanceGrid.FinancialData.Infrastructure.Services;

public class StockMarketTimeService : IStockMarketTime
{
    public bool IsMarketOpen(DateTime date)
    {
        var day = (int)date.DayOfWeek;
        var hours = date.Hour;
        var minutes = date.Minute;

        if (day < AppVariables.OPEN_MARKET_DAY || day > AppVariables.CLOSED_MARKET_DAY)
            return false;
        if (hours < AppVariables.OPEN_MARKET_HOURS || hours > AppVariables.CLOSED_MARKET_HOURS)
            return false;
        if (hours == AppVariables.OPEN_MARKET_HOURS && minutes < AppVariables.OPEN_MARKET_MINUTES)
            return false;

        return true;
    }

    public bool ShouldStockBeUpdated(Stock stock)
    {
        var now = DateTime.UtcNow;
        if (IsMarketOpen(now))
            return MinutesPassed(now, stock.UpdatedTime) > AppVariables.MINUTE_INTERVAL_BETWEEN_UPDATE;

        var nextOpen = NextMarketOpenDate(stock.UpdatedTime);
        return now.CompareTo(nextOpen) > 0;
    }

    public DateTime LastMarketCloseDateTime()
    {
        var now = DateTime.UtcNow;
        var dateOnly = DateOnly.FromDateTime(now);

        while (!IsMarketWeekDay(dateOnly))
            dateOnly = dateOnly.AddDays(-1);

        var closeTime = new TimeOnly(AppVariables.CLOSED_MARKET_HOURS, 0);
        return new DateTime(dateOnly, closeTime);
    }

    private bool IsMarketWeekDay(DateOnly date)
    {
        var day = (int)date.DayOfWeek;
        return day >= AppVariables.OPEN_MARKET_DAY && day <= AppVariables.CLOSED_MARKET_DAY;
    }

    private static int MinutesPassed(DateTime first, DateTime second)
    {
        var max = first > second ? first : second;
        var min = first > second ? second : first;
        return (int)Math.Floor(max.Subtract(min).TotalMinutes);
    }

    private DateTime NextMarketOpenDate(DateTime date)
    {
        var marketOpen = new DateTime(
            date.Year, date.Month, date.Day,
            AppVariables.OPEN_MARKET_HOURS, AppVariables.OPEN_MARKET_MINUTES, 0);

        while (marketOpen.CompareTo(date) < 0)
            marketOpen = marketOpen.AddDays(1);
        while (!IsMarketWeekDay(DateOnly.FromDateTime(marketOpen)))
            marketOpen = marketOpen.AddDays(1);

        return marketOpen;
    }
}
