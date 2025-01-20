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
        public required double FiftyDayAverage { get; set; }
        public required double TwoHundredDayAverage { get; set; }
        public required string FiftyTwoWeekRange { get; set; }
        public required double FiftyTwoWeekLow { get; set; }
        public required double FiftyTwoWeekHigh { get; set; }
        public required DateTime UpdatedTime { get; set; }
        public string? AnalystRating { get; set; }
        public StockAnalysis? Analysis { get; set; }
        public required List<StockNotification> StockNotifications { get; set; }
    }
}