using StocksAbstractions.Models.Stocks;
using StocksProvider.Models.YahooFinance127;

namespace StocksProvider.Generators
{
	public static class StockAnalysisGenerator
	{
		public static StockAnalysis Generate(string symbol, Finance127AnalysisResponse response)
		{
            var stockAnalysis = new StockAnalysis
            {
                Symbol = symbol,
                UpdatedTime = DateTime.UtcNow,
                TargetHighPrice = response.TargetHighPrice.Raw,
                TargetLowPrice = response.TargetLowPrice.Raw,
                TargetMeanPrice = response.TargetMeanPrice.Raw,
                TargetMedianPrice = response.TargetMedianPrice.Raw
            };

            return stockAnalysis;
        }
	}
}