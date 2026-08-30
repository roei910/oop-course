using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Persistence.Repositories;

public class SearchResultRepository : ISearchResultRepository
{
    private readonly IDbContextFactory<FinancialDataDbContext> _contextFactory;
    private readonly ILogger<SearchResultRepository> _logger;

    public SearchResultRepository(
        IDbContextFactory<FinancialDataDbContext> contextFactory,
        ILogger<SearchResultRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<List<StockSearchResult>> SearchStockByTermAsync(string searchTerm)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var searchResult = await context.SearchResults
            .Include(s => s.StockSearchResults)
            .FirstOrDefaultAsync(s => s.SearchTerm == searchTerm);

        return searchResult?.StockSearchResults ?? [];
    }

    public async Task CreateAsync(SearchResult searchResult)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.SearchResults.Add(searchResult);
        await context.SaveChangesAsync();
    }
}
