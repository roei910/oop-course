namespace Library.Models
{
    public class DatabaseSettings
    {
        public required string DatabaseName { get; set; }
        public required string StocksCollectionName { get; set; }
        public required string UsersCollectionName { get; set; }
        public required string MarketTrendsCollectionName { get; set; }
        public required string SearchResultsCollectionName { get; set; }
    }
}