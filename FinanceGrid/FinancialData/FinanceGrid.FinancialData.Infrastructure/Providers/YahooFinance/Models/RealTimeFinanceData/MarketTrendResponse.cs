namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.RealTimeFinanceData;

public class MarketTrendResponse
{
    public string? Symbol { get; set; }
    public string? Type { get; set; }
    public string? Name { get; set; }
    public double? Price { get; set; }
    public double? Change { get; set; }
    public double? Change_Percent { get; set; }
    public double? Previous_Close { get; set; }
    public string? Last_Update_Utc { get; set; }
    public string? Currency { get; set; }
    public string? Exchange { get; set; }
}
