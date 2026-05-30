using StocksApi.Abstractions.Models.SearchResults;

namespace StocksApi.Abstractions.Models
{
	public class SearchResult
	{
        public required string Id { get; set; }
        public required string SearchTerm { get; set; }
        public required List<StockSearchResult> StockSearchResults { get; set; }
    }
}
