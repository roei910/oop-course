using StocksAbstractions.Models.SearchResults;
using StocksProvider.Models.YahooFinance1;

namespace StocksProvider.Generators
{
	public static class StockSearchInformationGenerator
	{
        public static StockSearchResult Generate(Quote quote)
        {
            var stockSearchInformation = new StockSearchResult
            {
                Symbol = quote.Symbol,
                Name = quote.ShortName,
                ExchDisp = quote.ExchDisp,
                TypeDisp = quote.TypeDisp
            };

            return stockSearchInformation;
        }
    }
}

