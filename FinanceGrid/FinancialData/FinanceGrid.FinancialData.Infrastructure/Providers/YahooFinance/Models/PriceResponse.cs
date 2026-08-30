namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models;

public class PriceResponse
{
    public string? Symbol { get; set; }
    public string? ShortName { get; set; }
    public string? LongName { get; set; }
    public string? FullExchangeName { get; set; }
    public string? Exchange { get; set; }
    public string? Currency { get; set; }
    public string? QuoteType { get; set; }
    public string? TypeDisp { get; set; }
    public string? AverageAnalystRating { get; set; }

    public double RegularMarketPrice { get; set; }
    public double RegularMarketChange { get; set; }
    public double RegularMarketChangePercent { get; set; }
    public double RegularMarketPreviousClose { get; set; }
    public double RegularMarketOpen { get; set; }
    public double RegularMarketDayLow { get; set; }
    public double RegularMarketDayHigh { get; set; }
    public string? RegularMarketDayRange { get; set; }
    public long RegularMarketVolume { get; set; }

    public double FiftyDayAverage { get; set; }
    public double TwoHundredDayAverage { get; set; }
    public string? FiftyTwoWeekRange { get; set; }
    public double FiftyTwoWeekLow { get; set; }
    public double FiftyTwoWeekHigh { get; set; }

    public double TargetPriceHigh { get; set; }
    public double TargetPriceLow { get; set; }
    public double TargetPriceMean { get; set; }
    public double TargetPriceMedian { get; set; }

    public double ForwardPE { get; set; }
    public double EpsCurrentYear { get; set; }
    public double EpsForward { get; set; }
    public double EpsTrailingTwelveMonths { get; set; }
}
