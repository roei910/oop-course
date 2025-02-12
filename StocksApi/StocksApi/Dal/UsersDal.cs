using MongoDB.Driver;
using StocksApi.Interfaces;
using Library.Models;
using Library.Interfaces;
using Library.Models.Users;
using Library.Models.Shares;
using Library.Models.Users.StockNotes;
using MongoDB.Bson;
using Library.Models.Users.Notifications;

namespace StocksApi.Dal
{
    public class UsersDal : IUsersDal
    {
        private readonly IMongoCollection<User> _collection;

        public UsersDal(IAppConfiguration appConfiguration)
        {
            var databaseSettings = appConfiguration
                .Get<DatabaseSettings>(ConfigurationKeys.DatabaseSettingsSection);

            var connectionString = appConfiguration.Get<string>(ConfigurationKeys.ConnectionStringSection);

            var mongoClient = new MongoClient(connectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.DatabaseName);

            _collection = mongoDatabase.GetCollection<User>(
                databaseSettings.UsersCollectionName);
        }

        public async Task<List<User>> FindAllAsync()
        {
            var filter = Builders<User>.Filter.Empty;
            var users = await (await _collection.FindAsync(filter)).ToListAsync();

            return users;
        }

        public async Task<User?> FindOneByIdAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Id, id);
            var foundUser = await (await _collection.FindAsync(filter)).FirstOrDefaultAsync();

            return foundUser;
        }

        public async Task CreateAsync(User user)
        {
            await _collection.InsertOneAsync(user);
        }

        public async Task UpdateAsync(string id, User updatedUser)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedUser);
        }

        public async Task RemoveAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<User?> FindOneByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Email, email);
            var foundUser = await (await _collection.FindAsync(filter)).FirstOrDefaultAsync();

            return foundUser;
        }

        public async Task UpdatePasswordAsync(string email, string updatedPassword)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Email, email);
            var updatePassword = Builders<User>.Update.Set(user => user.Password, updatedPassword);

            await _collection.UpdateOneAsync(filter, updatePassword);
        }

        public async Task AddNotificationAsync(string userEmail, StockNotification notification)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Email, userEmail);
            var update = Builders<User>.Update.AddToSet(user => user.StockNotifications, notification);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task AddStockListAsync(string userEmail, string listName)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Email, userEmail);
            var update = Builders<User>.Update
            .Set(user =>
                    user.WatchingStocksByListName[listName],
                    new Dictionary<string, WatchingStock>());

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task RemoveStockListAsync(string email, string listName)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Email, email);
            var update = Builders<User>.Update
                .Unset(user => user.WatchingStocksByListName[listName]);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task NotifyUserAsync(string userEmail, string notificationId)
        {
            var filter = Builders<User>.Filter;

            var notificationFilter = filter.And(
                filter.Eq(user => user.Email, userEmail),
                filter.ElemMatch(user => user.StockNotifications, stockNotification => stockNotification.Id == notificationId));

            var update = Builders<User>.Update.Set("StockNotifications.$.ShouldBeNotified", true);

            await _collection.UpdateOneAsync(notificationFilter, update);
        }

        public async Task RemoveNotificationAsync(string userId, string notificationId)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Id, userId);
            var update = Builders<User>.Update
                .PullFilter(user => user.StockNotifications, notification => notification.Id == notificationId);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task AddUserStockNoteAsync(string userEmail, string stockSymbol, UserStockNote userStockNote)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Email, userEmail);
            var update = Builders<User>.Update
                .AddToSet(user => user.UserStockNotesBySymbol[stockSymbol], userStockNote);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task RemoveUserStockNoteAsync(string userEmail, string stockSymbol, string noteId)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Email, userEmail);

            var user = await _collection.FindSync(filter).FirstAsync();

            if (user.UserStockNotesBySymbol[stockSymbol].Count == 1)
            {
                var removeEmptyListSymbol = Builders<User>.Update
                    .Unset(user => user.UserStockNotesBySymbol[stockSymbol]);

                await _collection.UpdateOneAsync(filter, removeEmptyListSymbol);

                return;
            }

            var update = Builders<User>.Update
                .PullFilter(user => user.UserStockNotesBySymbol[stockSymbol], note => note.Id == noteId);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateUserStockNoteAsync(UserStockNoteUpdateRequest noteUpdateRequest)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Email, noteUpdateRequest.UserEmail);

            var notePath = "UserStockNotesBySymbol.AAPL.$[note]";
            var update = Builders<User>.Update
                .Set($"{notePath}.Note", noteUpdateRequest.Note)
                .Set($"{notePath}.LastUpdateTime", DateTime.UtcNow);

            var arrayFilters = new List<ArrayFilterDefinition>
            {
                new BsonDocumentArrayFilterDefinition<UserStockNote>(new BsonDocument("note._id", ObjectId.Parse(noteUpdateRequest.Id)))
            };

            var result = await _collection.UpdateOneAsync(filter, update, options: new UpdateOptions
            {
                ArrayFilters = arrayFilters
            });
        }
    }
}