using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.Finance1;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.Finance127;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public static class StockMapper
{
    public static Stock MapToStock(PriceResponse price)
    {
        return new Stock
        {
            Id = Guid.NewGuid().ToString(),
            Symbol = price.Symbol ?? string.Empty,
            Name = price.ShortName ?? price.LongName ?? string.Empty,
            Price = price.RegularMarketPrice,
            RegularMarketPreviousClose = price.RegularMarketPreviousClose,
            RegularMarketOpen = price.RegularMarketOpen,
            RegularMarketDayLow = price.RegularMarketDayLow,
            RegularMarketDayHigh = price.RegularMarketDayHigh,
            RegularMarketDayRange = price.RegularMarketDayRange,
            RegularMarketChange = price.RegularMarketChange,
            RegularMarketChangePercent = price.RegularMarketChangePercent,
            RegularMarketVolume = price.RegularMarketVolume,
            FiftyDayAverage = price.FiftyDayAverage,
            TwoHundredDayAverage = price.TwoHundredDayAverage,
            FiftyTwoWeekRange = price.FiftyTwoWeekRange,
            FiftyTwoWeekLow = price.FiftyTwoWeekLow,
            FiftyTwoWeekHigh = price.FiftyTwoWeekHigh,
            ForwardPE = price.ForwardPE,
            EpsCurrentYear = price.EpsCurrentYear,
            EpsForward = price.EpsForward,
            FullExchangeName = price.FullExchangeName,
            AnalystRating = price.AverageAnalystRating,
            TargetPriceLow = price.TargetPriceLow,
            TargetPriceHigh = price.TargetPriceHigh,
            TargetPriceMean = price.TargetPriceMean,
            TargetPriceMedian = price.TargetPriceMedian,
            UpdatedTime = DateTime.UtcNow
        };
    }

    public static StockSearchResult MapToStockSearchResult(Quote quote)
    {
        return new StockSearchResult
        {
            Symbol = quote.Symbol,
            Name = quote.ShortName,
            ExchDisp = quote.ExchDisp,
            TypeDisp = quote.TypeDisp
        };
    }

    public static StockSearchResult MapToStockSearchResult(PriceResponse price)
    {
        return new StockSearchResult
        {
            Symbol = price.Symbol ?? string.Empty,
            Name = price.ShortName ?? price.LongName ?? string.Empty,
            ExchDisp = price.FullExchangeName ?? string.Empty,
            TypeDisp = price.TypeDisp ?? string.Empty
        };
    }

    public static StockAnalysis MapToStockAnalysis(string symbol, Finance127AnalysisResponse response)
    {
        return new StockAnalysis
        {
            Symbol = symbol,
            UpdatedTime = DateTime.UtcNow,
            TargetHighPrice = response.TargetHighPrice.Raw,
            TargetLowPrice = response.TargetLowPrice.Raw,
            TargetMeanPrice = response.TargetMeanPrice.Raw,
            TargetMedianPrice = response.TargetMedianPrice.Raw
        };
    }
}
