using YahooFinance.Models.Price;

namespace YahooFinance.Models.YahooFinance1
{
	public class StocksResult
	{
		public required List<PriceResponse> Result { get; set; }
	}
}