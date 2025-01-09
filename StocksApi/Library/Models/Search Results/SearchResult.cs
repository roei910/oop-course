using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.SearchResults
{
    [BsonIgnoreExtraElements]
	public class SearchResult
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        public required string SearchTerm { get; set; }
        public required List<StockSearchResult> StockSearchResults { get; set; }
    }
}