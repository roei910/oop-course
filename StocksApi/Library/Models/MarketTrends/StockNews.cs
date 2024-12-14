namespace Library.Models.MarketTrends
{
    public class StockNews
    {
        public string? ArticleTitle { get; set; }
        public string? ArticleUrl { get; set; }
        public string? ArticlePhotoUrl { get; set; }
        public string? Source { get; set; }
        public string? PostTimeUtc { get; set; }
        public List<StockTrend>? StocksInNews { get; set; }
    }
}