using Library.Models.SearchResults;
using YahooFinance.Models.YahooFinance1;

namespace YahooFinance.Generators
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

