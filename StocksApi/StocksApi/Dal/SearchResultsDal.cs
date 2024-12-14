using Library.Interfaces;
using Library.Models;
using Library.Models.SearchResults;
using MongoDB.Driver;
using StocksApi.Interfaces;

namespace StocksApi.Dal
{
    public class SearchResultsDal : ISearchResultsDal
    {
        private readonly IMongoCollection<SearchResult> _collection;

        public SearchResultsDal(IAppConfiguration appConfiguration)
        {
            var databaseSettings = appConfiguration
                .Get<DatabaseSettings>(ConfigurationKeys.DatabaseSettingsSection);

            var connectionString = appConfiguration.Get<string>(ConfigurationKeys.ConnectionStringSection);

            var mongoClient = new MongoClient(connectionString);

            var mongoDatabase = mongoClient.GetDatabase(
               databaseSettings.DatabaseName);

            _collection = mongoDatabase.GetCollection<SearchResult>(
                databaseSettings.SearchResultsCollectionName);
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