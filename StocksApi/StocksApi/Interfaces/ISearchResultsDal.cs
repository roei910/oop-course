using Library.Models.SearchResults;

namespace StocksApi.Interfaces
{
	public interface ISearchResultsDal
	{
        Task CreateAsync(SearchResult item);
        Task<SearchResult?> FindBySearchTermAsync(string searchTerm);
    }
}