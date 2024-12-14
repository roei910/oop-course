using StocksApi.Generators;
using StocksApi.Interfaces;
using Library.Models;
using Library.Models.Shares;
using Library.Models.Users;

namespace StocksApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUsersDal _usersDal;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IStockRepository _stockRepository;

        public UserRepository(IUsersDal usersDal,
            IPasswordHasher passwordHasher,
            IStockRepository stockRepository)
        {
            _usersDal = usersDal;
            _passwordHasher = passwordHasher;
            _stockRepository = stockRepository;
        }

        public async Task AddUserAsync(UserDetails userDetails)
        {
            var foundUser = await _usersDal.FindOneByEmailAsync(userDetails.Email);

            if (foundUser is not null)
                throw new Exception("Email already exists");

            var user = UserGenerator.Generate(userDetails);
            user.Password = _passwordHasher.HashPassword(userDetails.Password!);

            await _usersDal.CreateAsync(user);
        }

        public async Task RemoveUserAsync(string id)
        {
            await _usersDal.RemoveAsync(id);
        }

        public async Task<Share> AddShareAsync(string id, SharePurchase sharePurchase)
        {
            sharePurchase.StockSymbol = sharePurchase.StockSymbol.ToUpper();
            var share = ShareGenerator.Generate(sharePurchase);
            var foundUser = await _usersDal.FindOneByIdAsync(id) ?? throw new KeyNotFoundException("couldnt find user by id");

            var doesContainList = foundUser.WatchingStocksByListName.ContainsKey(sharePurchase.ListName);

            if (!doesContainList)
                foundUser.WatchingStocksByListName[sharePurchase.ListName] =
                    new Dictionary<string, WatchingStock> {
                        { sharePurchase.StockSymbol, WatchingStockGenerator.Generate(sharePurchase.StockSymbol) }
                    };

            var doesContainSymbol = foundUser.WatchingStocksByListName[sharePurchase.ListName]
                .ContainsKey(sharePurchase.StockSymbol);

            if (!doesContainSymbol)
                foundUser.WatchingStocksByListName[sharePurchase.ListName]
                    [sharePurchase.StockSymbol] = WatchingStockGenerator.Generate(sharePurchase.StockSymbol);

            var foundWatchingStock = foundUser
                .WatchingStocksByListName[sharePurchase.ListName][sharePurchase.StockSymbol];

            foundWatchingStock.PurchaseGuidToShares.Add(share.Id, share);

            await _usersDal.UpdateAsync(foundUser.Id!, foundUser);

            return share;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = await _usersDal.FindAllAsync();

            return users;
        }

        public async Task<User?> GetAsync(string email)
        {
            var foundUser = await _usersDal.FindOneByEmailAsync(email);

            return foundUser;
        }

        public async Task<bool> ConnectUserAsync(UserCredentials user)
        {
            var foundUser = await _usersDal.FindOneByEmailAsync(user.Email);

            if (foundUser is null)
                return false;

            var isVerified = _passwordHasher.VerifyPassword(user.Password, foundUser.Password!);

            return isVerified;
        }

        public async Task AddWatchingStockAsync(string id, string listName, string stockSymbol)
        {
            stockSymbol = stockSymbol.ToUpper();
            var foundStock = await _stockRepository.GetStockBySymbolAsync(stockSymbol);

            _ = foundStock ?? throw new Exception("couldn't find stock in db");

            var foundUser = await _usersDal.FindOneByIdAsync(id) ?? throw new KeyNotFoundException("couldnt find user by id");

            var watchingStock = WatchingStockGenerator.Generate(stockSymbol);

            foundUser!.WatchingStocksByListName[listName].Add(stockSymbol, watchingStock);
            await _usersDal.UpdateAsync(id, foundUser!);
        }

        public async Task RemoveShareAsync(ShareSale shareSale)
        {
            var foundUser = await _usersDal.FindOneByEmailAsync(shareSale.UserEmail) ??
                throw new KeyNotFoundException("couldnt find user by id");

            var watchingStock = foundUser.WatchingStocksByListName[shareSale.ListName][shareSale.StockSymbol];
            var hasShare = watchingStock?.PurchaseGuidToShares.ContainsKey(shareSale.SharePurchaseGuid) ?? false;

            if(!hasShare)
                throw new Exception("couldnt find share by purchase id");

            watchingStock!.PurchaseGuidToShares.Remove(shareSale.SharePurchaseGuid);
            
            await _usersDal.UpdateAsync(foundUser.Id!, foundUser);
        }

        public async Task RemoveWatchingStockAsync(string id, string listName, string stockSymbol)
        {
            var foundUser = await _usersDal.FindOneByIdAsync(id) ?? throw new KeyNotFoundException("couldnt find user by id");
            foundUser!.WatchingStocksByListName[listName].Remove(stockSymbol);
            
            await _usersDal.UpdateAsync(id, foundUser);
        }

        public async Task UpdateShareNoteAsync(WatchingStockAction watchingStockAction)
        {
            var foundUser = await _usersDal.FindOneByEmailAsync(watchingStockAction.Email) ??
                throw new KeyNotFoundException("couldnt find user by email");

            foundUser.WatchingStocksByListName[watchingStockAction.ListName]
                [watchingStockAction.StockSymbol].Note = watchingStockAction.Note;

            await _usersDal.UpdateAsync(foundUser.Id!, foundUser);
        }

        public async Task AddStockListAsync(StockListDetails stockListDetails)
        {
            var email = stockListDetails.UserEmail;
            var foundUser = await _usersDal.FindOneByEmailAsync(email) ?? throw new KeyNotFoundException("user not found");

            var listName = stockListDetails.ListName;
            if (foundUser.WatchingStocksByListName.ContainsKey(listName))
                throw new Exception("Stock list already exists");

            await _usersDal.AddStockListAsync(email, listName);
        }

        public async Task RemoveStockListAsync(StockListDetails stockListDetails)
        {
            var email = stockListDetails.UserEmail;
            _ = await _usersDal.FindOneByEmailAsync(email) ?? throw new KeyNotFoundException("user not found");

            await _usersDal.RemoveStockListAsync(email, stockListDetails.ListName);
        }

        public async Task AddNotificationAsync(StockNotification notification)
        {
            var email = notification.UserEmail;
            _ = await _usersDal.FindOneByEmailAsync(email) ?? throw new KeyNotFoundException("user not found");

            await _usersDal.AddNotificationAsync(email, notification);
        }

        public async Task RemoveNotificationAsync(string userId, string notificationId)
        {
            await _usersDal.RemoveNotificationAsync(userId, notificationId);
        }

        public async Task ShowNotificationAsync(StockNotification notification)
        {
            var email = notification.UserEmail;
            _ = await _usersDal.FindOneByEmailAsync(email) ?? throw new KeyNotFoundException("user not found");

            await _usersDal.NotifyUserAsync(notification.UserEmail, notification.Id!);
        }

        public async Task UpdatePasswordAsync(PasswordUpdateRequest passwordUpdateRequest)
        {
            var email = passwordUpdateRequest.Email;
            _ = await _usersDal.FindOneByEmailAsync(email) ?? throw new KeyNotFoundException("user not found");

            var hashedPassword = _passwordHasher.HashPassword(passwordUpdateRequest.Password);
            await _usersDal.UpdatePasswordAsync(email, hashedPassword);
        }
    }
}