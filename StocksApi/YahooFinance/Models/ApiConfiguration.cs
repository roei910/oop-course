namespace YahooFinance.Models
{
	public class ApiConfiguration
	{
        public required string BaseUrl { get; set; }
        public required Dictionary<string, string> Headers { get; set; }
    }
}