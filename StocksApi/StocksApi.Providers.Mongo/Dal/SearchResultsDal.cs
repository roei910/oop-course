using StocksApi.Providers.Mongo;
using StocksAbstractions.Models;
using MongoDB.Driver;
using StocksAbstractions.Dal;

namespace StocksApi.Providers.Mongo.Dal
{
    public class SearchResultsDal : ISearchResultsDal
    {
        private readonly IMongoCollection<SearchResult> _collection;

        public SearchResultsDal(IMongoDbContext mongoDbContext)
        {
            _collection = mongoDbContext.GetCollection<SearchResult>("SearchResults");
        }

        public async Task CreateAsync(SearchResult newSearchResult) =>
            await _collection.InsertOneAsync(newSearchResult);

        public async Task<SearchResult?> FindBySearchTermAsync(string searchTerm)
        {
            var filterSearchTerm = Builders<SearchResult>.Filter.Eq(searchResult => searchResult.SearchTerm, searchTerm);
            var searchResult = await (await _collection.FindAsync(filterSearchTerm)).FirstOrDefaultAsync();

            return searchResult;
        }
    }
}
