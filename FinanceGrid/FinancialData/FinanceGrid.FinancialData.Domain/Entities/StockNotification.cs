namespace FinanceGrid.FinancialData.Domain.Entities;

public class StockNotification
{
    public string? Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string StockSymbol { get; set; } = string.Empty;
    public double PriceThreshold { get; set; }
    public string Condition { get; set; } = string.Empty;
}
