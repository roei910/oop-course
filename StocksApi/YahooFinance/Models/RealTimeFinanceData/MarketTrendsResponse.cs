using Library.Models.Stocks;

namespace YahooFinance.Models.RealTimeFinanceData
{
	public class MarketTrendsResponse
	{
		public required List<MarketTrendResponse> Trends { get; set; }
		public required List<MarketNewsResponse> News { get; set; }
	}
}