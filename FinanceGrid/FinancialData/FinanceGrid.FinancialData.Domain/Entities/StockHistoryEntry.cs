namespace FinanceGrid.FinancialData.Domain.Entities;

public class StockHistoryEntry
{
    public string? Id { get; set; }
    public string StockSymbol { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public double PriceOpen { get; set; }
    public double PriceClose { get; set; }
    public double DayLow { get; set; }
    public double DayHigh { get; set; }
    public string DayRange { get; set; } = string.Empty;
    public long DayVolume { get; set; }
}
