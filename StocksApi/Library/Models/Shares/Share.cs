using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.Shares
{
    [BsonIgnoreExtraElements]
    public class Share
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        public required double PurchasingPrice { get; set; }
        public required DateTime PurchaseDate { get; set; }
        public required double Amount { get; set; }
    }
}