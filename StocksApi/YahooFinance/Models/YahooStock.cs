namespace YahooFinance.Models
{
	public class PriceResponse
	{
		public required string Symbol { get; set; }
        public required string ShortName { get; set; }
        public required double RegularMarketPrice { get; set; }
		public required double FiftyDayAverage { get; set; }
		public required double TwoHundredDayAverage { get; set; }
		public required string FiftyTwoWeekRange { get; set; }
		public required double FiftyTwoWeekLow { get; set; }
		public required double FiftyTwoWeekHigh { get; set; }
		public string? AverageAnalystRating { get; set; }
    }
}