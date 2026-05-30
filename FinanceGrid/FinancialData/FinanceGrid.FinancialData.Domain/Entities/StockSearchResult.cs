namespace FinanceGrid.FinancialData.Domain.Entities;

public class StockSearchResult
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ExchDisp { get; set; } = string.Empty;
    public string TypeDisp { get; set; } = string.Empty;
}
