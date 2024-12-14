using Library.Models.SearchResults;
using MongoDB.Bson;

namespace StocksApi.Generators
{
	public static class SearchResultGenerator
	{
		public static SearchResult Generate(string searchTerm, List<StockSearchResult> stockSearchResults)
		{
			var searchResult = new SearchResult
			{
				Id = ObjectId.GenerateNewId().ToString(),
				SearchTerm = searchTerm,
				StockSearchResults = stockSearchResults
			};

			return searchResult;
		}
	}
}