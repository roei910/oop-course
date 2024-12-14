namespace Library.Models.Shares
{
    public class SharePurchase
	{
        public required string UserEmail { get; set; }
        public required string StockSymbol { get; set; }
        public required DateTime PurchaseDate { get; set; }
        public required double PurchasingPrice { get; set; }
        public required double Amount { get; set; }
        public required string ListName { get; set; }
    }
}