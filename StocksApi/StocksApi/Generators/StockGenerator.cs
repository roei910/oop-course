using Library.Models;
using Library.Models.Users;
using MongoDB.Bson;
using YahooFinance.Models;

namespace StocksApi.Generators
{
    public class StockGenerator
	{
        public static Stock? Generate(PriceResponse? yahooStock)
        {
            if (yahooStock is null)
                return null;

            var stock = new Stock
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = yahooStock.ShortName,
                Symbol = yahooStock.Symbol,
                Price = yahooStock.RegularMarketPrice,
                FiftyDayAverage = yahooStock.FiftyDayAverage,
                FiftyTwoWeekHigh = yahooStock.FiftyTwoWeekHigh,
                FiftyTwoWeekLow = yahooStock.FiftyTwoWeekLow,
                FiftyTwoWeekRange = yahooStock.FiftyTwoWeekRange,
                TwoHundredDayAverage = yahooStock.TwoHundredDayAverage,
                UpdatedTime = DateTime.UtcNow,
                AnalystRating = yahooStock.AverageAnalystRating,
                StockNotifications = new List<StockNotification>()
            };

            return stock;
        }
    }
}