using FinanceGrid.Users.Application.Interfaces;
using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.Domain.Interfaces;

namespace FinanceGrid.Users.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetByEmailAsync(string email) => await _userRepository.GetAsync(email);

    public async Task<List<User>> GetAllAsync() => await _userRepository.GetAllAsync();

    public async Task<User?> ConnectUserAsync(string email, string password)
    {
        var user = await _userRepository.GetAsync(email);
        if (user is null) return null;

        var authenticated = await _userRepository.ConnectUserAsync(email, password);
        return authenticated ? user : null;
    }

    public async Task CreateUserAsync(string firstName, string lastName, string email, string password)
    {
        await _userRepository.AddUserAsync(firstName, lastName, email, password);
    }

    public async Task UpdatePasswordAsync(string email, string password)
    {
        await _userRepository.UpdatePasswordAsync(email, password);
    }

    public async Task DeleteUserAsync(string email)
    {
        var user = await _userRepository.GetAsync(email);
        if (user?.Id is not null)
            await _userRepository.RemoveUserAsync(user.Id);
    }
}
