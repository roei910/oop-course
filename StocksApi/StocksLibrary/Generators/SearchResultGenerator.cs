using StocksAbstractions.Models;
using StocksAbstractions.Models.SearchResults;

namespace StocksLibrary.Generators
{
	public static class SearchResultGenerator
	{
		public static SearchResult Generate(string searchTerm, List<StockSearchResult> stockSearchResults)
		{
			var searchResult = new SearchResult
			{
				Id = Guid.NewGuid().ToString(),
				SearchTerm = searchTerm,
				StockSearchResults = stockSearchResults
			};

			return searchResult;
		}
	}
}
