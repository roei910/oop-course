using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.Users
{
	public class UserStockNote
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
		public required string Note { get; set; }
        public required DateTime CreationTime { get; set; }
        public required DateTime LastUpdateTime { get; set; }
    }
}