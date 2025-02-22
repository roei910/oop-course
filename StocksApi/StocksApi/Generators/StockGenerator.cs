using Library.Models;
using Library.Models.Users.Notifications;
using MongoDB.Bson;
using YahooFinance.Models.Price;

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
                UpdatedTime = DateTime.UtcNow,
                StockNotifications = new List<StockNotification>(),
                Name = yahooStock.ShortName!,
                Symbol = yahooStock.Symbol!,
                Price = yahooStock.RegularMarketPrice,
                FiftyDayAverage = yahooStock.FiftyDayAverage,
                FiftyTwoWeekHigh = yahooStock.FiftyTwoWeekHigh,
                FiftyTwoWeekLow = yahooStock.FiftyTwoWeekLow,
                FiftyTwoWeekRange = yahooStock.FiftyTwoWeekRange,
                TwoHundredDayAverage = yahooStock.TwoHundredDayAverage,
                FullExchangeName = yahooStock.FullExchangeName,
                AnalystRating = yahooStock.AverageAnalystRating,
                ForwarPE = yahooStock.ForwardPE,
                EpsCurrentYear = yahooStock.EpsCurrentYear,
                EpsForward = yahooStock.EpsForward
            };

            return stock;
        }
    }
}