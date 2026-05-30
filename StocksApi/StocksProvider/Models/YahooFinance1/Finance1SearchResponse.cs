namespace StocksProvider.Models.YahooFinance1
{
    public class Finance1SearchResponse
	{
		public required List<Quote> Quotes { get; set; }
		public required List<StockNews> News { get; set; }
	}
}