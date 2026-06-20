namespace FinanceGrid.Users.Domain.Entities;

public class UserStockWatch
{
    public string? Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string ListName { get; set; } = string.Empty;
    public string StockSymbol { get; set; } = string.Empty;
    public string? Note { get; set; }
    public Dictionary<string, Share> PurchaseGuidToShares { get; set; } = [];
}
