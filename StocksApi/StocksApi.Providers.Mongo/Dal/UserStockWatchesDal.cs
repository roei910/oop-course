using MongoDB.Driver;
using UsersAbstractions.Dal;
using StocksApi.Providers.Mongo;
using UsersAbstractions.Models.Shares;

namespace StocksApi.Providers.Mongo.Dal
{
    public class UserStockWatchesDal : IUserStockWatchesDal
    {
        private readonly IMongoCollection<UserStockWatch> _collection;

        public UserStockWatchesDal(IMongoDbContext mongoDbContext)
        {
            _collection = mongoDbContext.GetCollection<UserStockWatch>("UserStockWatches");
        }

        public async Task InsertWatchAsync(UserStockWatch watch)
        {
            await _collection.InsertOneAsync(watch);
        }

        public async Task<List<UserStockWatch>> GetWatchesByEmailAsync(string email)
        {
            var filter = Builders<UserStockWatch>.Filter.Eq(w => w.UserEmail, email);
            return await (await _collection.FindAsync(filter)).ToListAsync();
        }

        public async Task<List<UserStockWatch>> GetWatchesByEmailAndListAsync(string email, string listName)
        {
            var filter = Builders<UserStockWatch>.Filter.And(
                Builders<UserStockWatch>.Filter.Eq(w => w.UserEmail, email),
                Builders<UserStockWatch>.Filter.Eq(w => w.ListName, listName));
            return await (await _collection.FindAsync(filter)).ToListAsync();
        }

        public async Task DeleteWatchAsync(string email, string listName, string stockSymbol)
        {
            var filter = Builders<UserStockWatch>.Filter.And(
                Builders<UserStockWatch>.Filter.Eq(w => w.UserEmail, email),
                Builders<UserStockWatch>.Filter.Eq(w => w.ListName, listName),
                Builders<UserStockWatch>.Filter.Eq(w => w.StockSymbol, stockSymbol));
            await _collection.DeleteOneAsync(filter);
        }

        public async Task DeleteWatchesByListAsync(string email, string listName)
        {
            var filter = Builders<UserStockWatch>.Filter.And(
                Builders<UserStockWatch>.Filter.Eq(w => w.UserEmail, email),
                Builders<UserStockWatch>.Filter.Eq(w => w.ListName, listName));
            await _collection.DeleteManyAsync(filter);
        }

        public async Task AddShareToWatchAsync(string email, string listName, string stockSymbol, string shareId, Share share)
        {
            var filter = Builders<UserStockWatch>.Filter.And(
                Builders<UserStockWatch>.Filter.Eq(w => w.UserEmail, email),
                Builders<UserStockWatch>.Filter.Eq(w => w.ListName, listName),
                Builders<UserStockWatch>.Filter.Eq(w => w.StockSymbol, stockSymbol));

            var update = Builders<UserStockWatch>.Update
                .Set($"PurchaseGuidToShares.{shareId}", share);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpsertWatchAndAddShareAsync(string email, string listName, string stockSymbol, string shareId, Share share)
        {
            var filter = Builders<UserStockWatch>.Filter.And(
                Builders<UserStockWatch>.Filter.Eq(w => w.UserEmail, email),
                Builders<UserStockWatch>.Filter.Eq(w => w.ListName, listName),
                Builders<UserStockWatch>.Filter.Eq(w => w.StockSymbol, stockSymbol));

            var update = Builders<UserStockWatch>.Update
                .SetOnInsert(w => w.UserEmail, email)
                .SetOnInsert(w => w.ListName, listName)
                .SetOnInsert(w => w.StockSymbol, stockSymbol)
                .SetOnInsert(w => w.Note, null)
                .SetOnInsert(w => w.PurchaseGuidToShares, new Dictionary<string, Share>())
                .Set($"PurchaseGuidToShares.{shareId}", share);

            await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task RemoveShareFromWatchAsync(string email, string listName, string stockSymbol, string shareId)
        {
            var filter = Builders<UserStockWatch>.Filter.And(
                Builders<UserStockWatch>.Filter.Eq(w => w.UserEmail, email),
                Builders<UserStockWatch>.Filter.Eq(w => w.ListName, listName),
                Builders<UserStockWatch>.Filter.Eq(w => w.StockSymbol, stockSymbol));

            var update = Builders<UserStockWatch>.Update
                .Unset($"PurchaseGuidToShares.{shareId}");

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateWatchNoteAsync(string email, string listName, string stockSymbol, string? note)
        {
            var filter = Builders<UserStockWatch>.Filter.And(
                Builders<UserStockWatch>.Filter.Eq(w => w.UserEmail, email),
                Builders<UserStockWatch>.Filter.Eq(w => w.ListName, listName),
                Builders<UserStockWatch>.Filter.Eq(w => w.StockSymbol, stockSymbol));

            var update = Builders<UserStockWatch>.Update
                .Set(w => w.Note, note);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
