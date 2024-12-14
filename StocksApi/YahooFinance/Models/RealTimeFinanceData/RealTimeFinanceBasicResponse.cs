namespace YahooFinance.Models.RealTimeFinanceData
{
	public class RealTimeFinanceBasicResponse
	{
		public required string Status { get; set; }
		public required string Request_Id { get; set; }
		public required MarketTrendsResponse Data { get; set; }
	}
}