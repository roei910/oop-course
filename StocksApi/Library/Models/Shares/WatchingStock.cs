namespace Library.Models.Shares
{
    public class WatchingStock
	{
		public required Dictionary<string, Share> PurchaseGuidToShares { get; set; }
		public string? Note { get; set; }
	}
}