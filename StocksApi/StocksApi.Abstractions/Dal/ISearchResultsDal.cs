using StocksApi.Abstractions.Models;

namespace StocksApi.Abstractions.Dal
{
	public interface ISearchResultsDal
	{
        Task CreateAsync(SearchResult item);
        Task<SearchResult?> FindBySearchTermAsync(string searchTerm);
    }
}
