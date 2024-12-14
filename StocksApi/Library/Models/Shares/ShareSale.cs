namespace Library.Models.Shares
{
    public class ShareSale
	{
		public required string UserEmail { get; set; }
		public required string ListName { get; set; }
		public required string StockSymbol { get; set; }
		public required string SharePurchaseGuid { get; set; }
	}
}