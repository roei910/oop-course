using FinanceGrid.Users.Domain.Entities;

namespace FinanceGrid.Users.Application.Interfaces;

public interface IUserService
{
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();
    Task<User?> ConnectUserAsync(string email, string password);
    Task CreateUserAsync(string firstName, string lastName, string email, string password);
    Task UpdatePasswordAsync(string email, string password);
    Task DeleteUserAsync(string email);
}
