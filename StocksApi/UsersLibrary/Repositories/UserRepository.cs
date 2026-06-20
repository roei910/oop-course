using UsersLibrary.Generators;
using UsersAbstractions.Dal;
using UsersAbstractions.Repositories;
using UsersAbstractions.Models;
using UsersAbstractions.Models.Users;
using UsersAbstractions.Models.StockNotes;
using UsersAbstractions.Models.Notifications;
using UsersAbstractions.Services;

namespace UsersLibrary.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUsersDal _usersDal;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(IUsersDal usersDal,
            IPasswordHasher passwordHasher)
        {
            _usersDal = usersDal;
            _passwordHasher = passwordHasher;
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

        public async Task<UserStockNote> AddStockNoteAsync(UserStockNoteRequest userStockNoteRequest)
        {
            var now = DateTime.UtcNow;

            var userStockNote = new UserStockNote
            {
                Id = Guid.NewGuid().ToString(),
                Note = userStockNoteRequest.Note,
                CreationTime = now,
                LastUpdateTime = now
            };

            await _usersDal.AddUserStockNoteAsync(userStockNoteRequest.UserEmail, userStockNoteRequest.StockSymbol, userStockNote);

            return userStockNote;
        }

        public async Task RemoveStockNoteAsync(string userEmail, string stockSymbol, string noteId)
        {
            await _usersDal.RemoveUserStockNoteAsync(userEmail, stockSymbol, noteId);
        }

        public async Task UpdateStockNoteAsync(UserStockNoteUpdateRequest noteUpdateRequest)
        {
            noteUpdateRequest.StockSymbol = noteUpdateRequest.StockSymbol.ToUpper();

            await _usersDal.UpdateUserStockNoteAsync(noteUpdateRequest);
        }
    }
}
