using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.ServerResponse
{
	public class ObjectIdResponse
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}