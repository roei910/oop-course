namespace YahooFinance.Models
{
	public class BasicResponse<T>
	{
		public required YahooMeta Meta { get; set; }
		public required List<T> Body { get; set; }
	}
}