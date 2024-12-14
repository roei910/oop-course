using Library.Models;
using Library.Interfaces;
using StocksApi.Interfaces;

namespace StocksApi.Services
{
    public class StockMarketTime : IStockMarketTime
    {
        private readonly Variables _variables;

        public StockMarketTime(IAppConfiguration financeConfiguration)
        {
            _variables = financeConfiguration.Get<Variables>(ConfigurationKeys.AppVariablesSection);
        }

        public bool IsMarketOpen(DateTime date)
        {
            var day = (int)date.DayOfWeek;
            var hours = date.Hour;
            var minutes = date.Minute;

            if (day < _variables.OPEN_MARKET_DAY || day > _variables.CLOSED_MARKET_DAY)
                return false;

            if (hours < _variables.OPEN_MARKET_HOURS || hours > _variables.CLOSED_MARKET_HOURS)
                return false;

            if (hours == _variables.OPEN_MARKET_HOURS && minutes < _variables.OPEN_MARKET_MINUTES)
                return false;

            return true;
        }

        public bool ShouldStockBeUpdated(Stock stock)
        {
            var now = DateTime.UtcNow;
            var dateUpdated = stock.UpdatedTime;

            if (IsMarketOpen(now))
                return MinutesPassed(now, dateUpdated) > _variables.MINUTE_INTERVAL_BETWEEN_UPDATE;

            var nextMarketOpenDate = NextMarketOpenDate(dateUpdated);

            if (now.CompareTo(nextMarketOpenDate) > 0)
                return true;

            return false;
        }

        private bool IsMarketWeekDay(DateTime date)
        {
            var day = (int)date.DayOfWeek;

            return day >= _variables.OPEN_MARKET_DAY &&
                day <= _variables.CLOSED_MARKET_DAY;
        }

        private static TimeSpan DaysPassed(DateTime dateFirst, DateTime dateSecond)
        {
            var maxDay = dateFirst > dateSecond ? dateFirst : dateSecond;
            var minDay = dateFirst > dateSecond ? dateSecond : dateFirst;
            var subTract = maxDay.Subtract(minDay);

            return subTract;
        }

        private static int MinutesPassed(DateTime dateFirst, DateTime dateSecond)
        {
            var daysPassed = DaysPassed(dateFirst, dateSecond);
            
            return (int)Math.Floor(daysPassed.TotalMinutes);
        }

        private DateTime NextMarketOpenDate(DateTime date)
        {
            var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
            var marketOpenTime = new TimeOnly(_variables.OPEN_MARKET_HOURS, _variables.OPEN_MARKET_MINUTES);
            var nextMarketOpenDateTime = new DateTime(dateOnly, marketOpenTime);

            while (nextMarketOpenDateTime.CompareTo(date) < 0)
                nextMarketOpenDateTime = nextMarketOpenDateTime.AddDays(1);

            while (!IsMarketWeekDay(nextMarketOpenDateTime))
                nextMarketOpenDateTime = nextMarketOpenDateTime.AddDays(1);

            return nextMarketOpenDateTime;
        }
    }
}