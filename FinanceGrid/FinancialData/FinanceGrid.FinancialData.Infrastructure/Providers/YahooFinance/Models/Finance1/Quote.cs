namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.Finance1;

public class Quote
{
    public required string Symbol { get; set; }
    public required string ShortName { get; set; }
    public required string Exchange { get; set; }
    public required string ExchDisp { get; set; }
    public required string TypeDisp { get; set; }
}
