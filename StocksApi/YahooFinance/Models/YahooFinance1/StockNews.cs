namespace YahooFinance.Models.YahooFinance1
{
	public class StockNews
	{
		public required string Title { get; set; }
		public required string Publisher { get; set; }
		public required string Link { get; set; }
		public required string Type { get; set; }
		public required List<string> RelatedTickers { get; set; }
	}
}