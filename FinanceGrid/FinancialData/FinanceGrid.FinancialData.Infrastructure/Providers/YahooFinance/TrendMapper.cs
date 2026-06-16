using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.RealTimeFinanceData;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public static class TrendMapper
{
    public static MarketTrend MapToMarketTrend(string trendType, MarketTrendsResponse response)
    {
        return new MarketTrend
        {
            TrendName = trendType,
            TrendingStocks = response.Trends.Select(MapToStockTrend).ToList(),
            StockNewsItems = response.News.Select(MapToStockNews).ToList(),
            LastUpdatedTime = DateTime.UtcNow
        };
    }

    public static StockTrend MapToStockTrend(MarketTrendResponse trend)
    {
        return new StockTrend
        {
            Symbol = trend.Symbol,
            Type = trend.Type,
            Name = trend.Name,
            Price = trend.Price,
            Change = trend.Change,
            ChangePercent = trend.Change_Percent,
            PreviousClose = trend.Previous_Close,
            PreOrPostMarket = trend.Pre_or_Post_Market,
            PreOrPostMarketChange = trend.Pre_or_Post_Market_Change,
            PreOrPostMarketChangePercent = trend.Pre_or_Post_Market_Change_Percent,
            LastUpdateUtc = trend.Last_Update_Utc,
            Currency = trend.Currency,
            Exchange = trend.Exchange,
            ExchangeOpen = trend.Exchange_Open,
            ExchangeClose = trend.Exchange_Close,
            Timezone = trend.Timezone,
            CountryCode = trend.Country_Code
        };
    }

    public static StockNews MapToStockNews(MarketNewsResponse news)
    {
        return new StockNews
        {
            ArticleTitle = news.Article_Title,
            ArticleUrl = news.Article_Url,
            ArticlePhotoUrl = news.Article_Photo_Url,
            Source = news.Source,
            PostTimeUtc = news.Post_Time_Utc,
            StocksInNews = news.Stocks_In_News?.Select(MapToStockTrend).ToList()
        };
    }
}
