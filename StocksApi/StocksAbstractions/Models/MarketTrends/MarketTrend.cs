using StocksAbstractions.Models.MarketTrends;

namespace StocksAbstractions.Models
{
	public class MarketTrend
    {
        public string? Id { get; set; }
        public required string TrendName { get; set; }
		public required List<StockTrend> TrendingStocks { get; set; }
		public required List<StockNews> StockNews { get; set; }
		public required DateTime LastUpdatedTime { get; set; }
	}
}
