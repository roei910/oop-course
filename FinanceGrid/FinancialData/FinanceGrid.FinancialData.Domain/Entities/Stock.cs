namespace FinanceGrid.FinancialData.Domain.Entities;

public class Stock
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;

    public double Price { get; set; }
    public double RegularMarketPreviousClose { get; set; }
    public double RegularMarketOpen { get; set; }
    public double RegularMarketDayLow { get; set; }
    public double RegularMarketDayHigh { get; set; }
    public string? RegularMarketDayRange { get; set; }
    public double RegularMarketChange { get; set; }
    public double RegularMarketChangePercent { get; set; }
    public long RegularMarketVolume { get; set; }

    public double FiftyDayAverage { get; set; }
    public double TwoHundredDayAverage { get; set; }
    public string? FiftyTwoWeekRange { get; set; }
    public double FiftyTwoWeekLow { get; set; }
    public double FiftyTwoWeekHigh { get; set; }

    public double TargetPriceLow { get; set; }
    public double TargetPriceHigh { get; set; }
    public double TargetPriceMean { get; set; }
    public double TargetPriceMedian { get; set; }

    public double ForwardPE { get; set; }

    public double EpsCurrentYear { get; set; }
    public double EpsForward { get; set; }

    public string? FullExchangeName { get; set; }
    public string? AnalystRating { get; set; }

    public StockAnalysis? Analysis { get; set; }

    public DateTime UpdatedTime { get; set; }
    public DateOnly? LastHistoryUpdateDate { get; set; }

    public List<StockNotification> StockNotifications { get; set; } = [];
}
