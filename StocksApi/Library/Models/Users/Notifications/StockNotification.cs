using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.Users.Notifications
{
    public class StockNotification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string StockSymbol { get; set; }
        public required string UserEmail { get; set; }
        public required double TargetPrice { get; set; }
        public bool IsTargetBiggerThanOrEqual { get; set; }
        public bool ShouldBeNotified { get; set; }
    }
}