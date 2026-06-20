namespace StocksApi.Abstractions.Models.Shares
{
    public class UserStockWatch
    {
        public string? Id { get; set; }
        public required string UserEmail { get; set; }
        public required string ListName { get; set; }
        public required string StockSymbol { get; set; }
        public string? Note { get; set; }
        public required Dictionary<string, Share> PurchaseGuidToShares { get; set; }
    }
}
