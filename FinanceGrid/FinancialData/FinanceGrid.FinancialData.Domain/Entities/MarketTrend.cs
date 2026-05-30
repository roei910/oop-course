namespace FinanceGrid.FinancialData.Domain.Entities;

public class MarketTrend
{
    public string? Id { get; set; }
    public string TrendName { get; set; } = string.Empty;
    public List<StockTrend> TrendingStocks { get; set; } = [];
    public List<StockNews> StockNewsItems { get; set; } = [];
    public DateTime LastUpdatedTime { get; set; }
}
