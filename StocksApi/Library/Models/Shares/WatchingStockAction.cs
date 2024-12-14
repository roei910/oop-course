namespace Library.Models.Shares
{
    public class WatchingStockAction
	{
		public required string Email { get; set; }
		public required string ListName { get; set; }
		public required string StockSymbol { get; set; }
		public string? Note { get; set; }
	}
}