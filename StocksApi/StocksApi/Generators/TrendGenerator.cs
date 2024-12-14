using Library.Models.MarketTrends;
using Library.Models.Stocks;
using YahooFinance.Models.RealTimeFinanceData;

namespace StocksApi.Generators
{
	public class TrendGenerator
	{
		public static MarketTrend Generate(string trendName, MarketTrendsResponse marketTrendsResponse)
		{
			var stockMarketNewsList = marketTrendsResponse.News
				.Select(GenerateStockMarketNews)
				.ToList();

			var stockTrends = marketTrendsResponse.Trends
				.Select(GenerateStockTrend)
				.ToList();

            var marketTrend = new MarketTrend
			{
				LastUpdatedTime = DateTime.UtcNow,
				StockNews = stockMarketNewsList,
				TrendName = trendName,
				TrendingStocks = stockTrends
			};

			return marketTrend;
		}

		private static StockNews GenerateStockMarketNews(MarketNewsResponse marketNewsResponse)
		{
			var stocksInNews = marketNewsResponse.Stocks_In_News!
				.Select(GenerateStockTrend)
				.ToList();

			var stockMarketNews = new StockNews
			{
				StocksInNews = stocksInNews,
				Source = marketNewsResponse.Source,
                ArticlePhotoUrl = marketNewsResponse.Article_Photo_Url,
				ArticleTitle = marketNewsResponse.Article_Title,
				ArticleUrl = marketNewsResponse.Article_Url,
				PostTimeUtc = marketNewsResponse.Post_Time_Utc
			};

			return stockMarketNews;
        }

		private static StockTrend GenerateStockTrend(MarketTrendResponse marketTrendResponse)
		{
			var stockTrend = new StockTrend
			{
				Change = marketTrendResponse.Change,
				Symbol = marketTrendResponse.Symbol,
				ChangePercent = marketTrendResponse.Change_Percent,
				CountryCode = marketTrendResponse.Country_Code,
				Currency = marketTrendResponse.Currency,
				ExchangeClose = marketTrendResponse.Exchange_Close,
				Exchange = marketTrendResponse.Exchange,
				ExchangeOpen = marketTrendResponse.Exchange_Open,
				LastUpdateUtc = marketTrendResponse.Last_Update_Utc,
				Name = marketTrendResponse.Name,
				PreOrPostMarket = marketTrendResponse.Pre_Or_Post_Market,
				PreOrPostMarketChange = marketTrendResponse.Pre_Or_Post_Market_Change,
				PreOrPostMarketChangePercent = marketTrendResponse.Pre_Or_Post_Market_Change_Percent,
				PreviousClose = marketTrendResponse.Previous_Close,
				Price = marketTrendResponse.Price,
				Type = marketTrendResponse.Type,
				Timezone = marketTrendResponse.Timezone,
			};

			return stockTrend;
		}
	}
}