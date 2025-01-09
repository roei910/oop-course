using System.Text.Json.Serialization;
using Library.Models.Shares;
using Library.Models.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [JsonIgnore]
        public string? Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required Dictionary<string, Dictionary<string, WatchingStock>> WatchingStocksByListName { get; set; }
        public required List<StockNotification> StockNotifications { get; set; }
        public required Dictionary<string, List<UserStockNote>> UserStockNotesBySymbol { get; set; }
    }
}