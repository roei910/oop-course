using StocksAbstractions.Models;
using UsersAbstractions.Models.Notifications;
using StocksAbstractions.Models.Price;

namespace StocksLibrary.Generators
{
    public class StockGenerator
	{
        public static Stock? Generate(PriceResponse? yahooStock)
        {
            if (yahooStock is null)
                return null;

            var stock = new Stock
            {
                Id = Guid.NewGuid().ToString(),
                UpdatedTime = DateTime.UtcNow,
                StockNotifications = new List<StockNotification>(),
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
