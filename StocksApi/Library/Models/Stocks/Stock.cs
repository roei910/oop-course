using Library.Models.Stocks;
using Library.Models.Users.Notifications;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models
{
    [BsonIgnoreExtraElements]
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Symbol { get; set; }

        public required double Price { get; set; }
        public double RegularMarketPreviousClose { get; set; }
        public double RegularMarketOpen { get; set; }
        public double RegularMarketDayLow { get; set; }
        public double RegularMarketDayHigh { get; set; }
        public string? RegularMarketDayRange { get; set; }
        public double RegularMarketChange { get; set; }
        public double RegularMarketChangePercent { get; set; }
        public long RegularMarketVolume { get; set; }

        public required double FiftyDayAverage { get; set; }
        public required double TwoHundredDayAverage { get; set; }
        public string? FiftyTwoWeekRange { get; set; }
        public required double FiftyTwoWeekLow { get; set; }
        public required double FiftyTwoWeekHigh { get; set; }

        public double TargetPriceLow { get; set; }
        public double TargetPriceHigh { get; set; }
        public double TargetPriceMean { get; set; }
        public double TargetPriceMedian { get; set; }

        public double ForwardPE { get; set; }

        public double EpsCurrentYear { get; set; }
        public double EpsForward { get; set; }

        public required string? FullExchangeName { get; set; }

        public string? AnalystRating { get; set; }

        public StockAnalysis? Analysis { get; set; }

        public required DateTime UpdatedTime { get; set; }
        public DateOnly? LastHistoryUpdateDate { get; set; }

        public required List<StockNotification> StockNotifications { get; set; }
        public required List<StockHistory> History { get; set; }
    }
}