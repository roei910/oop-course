using Microsoft.EntityFrameworkCore;
using StocksAbstractions.Dal;
using StocksAbstractions.Models;

namespace StocksApi.Providers.Sqlite.Dal
{
    public class SearchResultsDal : ISearchResultsDal
    {
        private readonly IDbContextFactory<SqliteDbContext> _contextFactory;

        public SearchResultsDal(IDbContextFactory<SqliteDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(SearchResult item)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.SearchResults.Add(item);
            await context.SaveChangesAsync();
        }

        public async Task<SearchResult?> FindBySearchTermAsync(string searchTerm)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.SearchResults
                .Include(s => s.StockSearchResults)
                .FirstOrDefaultAsync(s => s.SearchTerm == searchTerm);
        }
    }
}
