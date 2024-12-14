namespace YahooFinance.Models
{
	public class YahooMeta
	{
		public required string Version { get; set; }
		public required int Status { get; set; }
		public required string Copywrite { get; set; }
		public required string Symbol { get; set; }
		public required string ProcessedTime { get; set; }
	}
}