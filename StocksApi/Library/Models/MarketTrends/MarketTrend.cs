using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.MarketTrends
{
    [BsonIgnoreExtraElements]
	public class MarketTrend
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string TrendName { get; set; }
		public required List<StockTrend> TrendingStocks { get; set; }
		public required List<StockNews> StockNews { get; set; }
		public required DateTime LastUpdatedTime { get; set; }
	}
}