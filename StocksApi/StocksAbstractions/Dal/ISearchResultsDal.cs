using StocksAbstractions.Models;

namespace StocksAbstractions.Dal
{
	public interface ISearchResultsDal
	{
        Task CreateAsync(SearchResult item);
        Task<SearchResult?> FindBySearchTermAsync(string searchTerm);
    }
}
