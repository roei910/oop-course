using StocksAbstractions.Models.Price;

namespace StocksProvider.Models.YahooFinance1
{
	public class StocksResult
	{
		public required List<PriceResponse> Result { get; set; }
	}
}