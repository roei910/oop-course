namespace FinanceGrid.Users.Domain.Entities;

public class StockNotification
{
    public string? Id { get; set; }
    public string StockSymbol { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public double TargetPrice { get; set; }
    public bool IsTargetBiggerThanOrEqual { get; set; }
    public bool ShouldBeNotified { get; set; }
}
