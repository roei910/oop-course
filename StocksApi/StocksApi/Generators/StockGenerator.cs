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
                History = new List<StockHistory>(),
                Name = yahooStock.ShortName!,
                Symbol = yahooStock.Symbol!,
                Price = yahooStock.RegularMarketPrice,
                RegularMarketPreviousClose = yahooStock.RegularMarketPreviousClose,
                RegularMarketOpen = yahooStock.RegularMarketOpen,
                RegularMarketDayLow = yahooStock.RegularMarketDayLow,
                RegularMarketDayHigh = yahooStock.RegularMarketDayHigh,
                RegularMarketDayRange = yahooStock.RegularMarketDayRange,
                RegularMarketChange = yahooStock.RegularMarketChange,
                RegularMarketChangePercent = yahooStock.RegularMarketChangePercent,
                RegularMarketVolume = yahooStock.RegularMarketVolume,
                FiftyDayAverage = yahooStock.FiftyDayAverage,
                FiftyTwoWeekHigh = yahooStock.FiftyTwoWeekHigh,
                FiftyTwoWeekLow = yahooStock.FiftyTwoWeekLow,
                FiftyTwoWeekRange = yahooStock.FiftyTwoWeekRange,
                TwoHundredDayAverage = yahooStock.TwoHundredDayAverage,
                FullExchangeName = yahooStock.FullExchangeName,
                AnalystRating = yahooStock.AverageAnalystRating,
                ForwardPE = yahooStock.ForwardPE,
                EpsCurrentYear = yahooStock.EpsCurrentYear,
                EpsForward = yahooStock.EpsForward,
                TargetPriceLow = yahooStock.TargetPriceLow,
                TargetPriceHigh = yahooStock.TargetPriceHigh,
                TargetPriceMean = yahooStock.TargetPriceMean,
                TargetPriceMedian = yahooStock.TargetPriceMedian,
            };

            return stock;
        }
    }
}