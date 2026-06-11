namespace FinanceGrid.FinancialData.Domain.Entities;

public class StockAnalysis
{
    public string Symbol { get; set; } = string.Empty;
    public DateTime UpdatedTime { get; set; }
    public double TargetHighPrice { get; set; }
    public double TargetLowPrice { get; set; }
    public double TargetMeanPrice { get; set; }
    public double TargetMedianPrice { get; set; }
}
