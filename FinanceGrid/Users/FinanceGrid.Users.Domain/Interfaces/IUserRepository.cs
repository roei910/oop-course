using FinanceGrid.Users.Domain.Entities;

namespace FinanceGrid.Users.Domain.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(string firstName, string lastName, string email, string password);
    Task<bool> ConnectUserAsync(string email, string password);
    Task<List<User>> GetAllAsync();
    Task<User?> GetAsync(string email);
    Task RemoveUserAsync(string id);
    Task UpdatePasswordAsync(string email, string password);
}
