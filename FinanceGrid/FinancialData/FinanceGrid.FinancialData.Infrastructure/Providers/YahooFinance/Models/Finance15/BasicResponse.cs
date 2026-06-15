namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.Finance15;

public class BasicResponse<T>
{
    public required YahooMeta Meta { get; set; }
    public required List<T> Body { get; set; }
}
