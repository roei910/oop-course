using FinanceGrid.Users.Application.Services;
using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.Domain.Interfaces;

namespace FinanceGrid.Users.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repoMock = new();
    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByEmailAsync_DelegatesToRepository()
    {
        var user = new User { Email = "test@test.com" };
        _repoMock.Setup(r => r.GetAsync("test@test.com")).ReturnsAsync(user);

        var result = await _service.GetByEmailAsync("test@test.com");

        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task GetAllAsync_DelegatesToRepository()
    {
        var users = new List<User> { new() { Email = "a@a.com" }, new() { Email = "b@b.com" } };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ConnectUserAsync_ValidCredentials_ReturnsUser()
    {
        var user = new User { Email = "test@test.com", Id = "u1" };
        _repoMock.Setup(r => r.GetAsync("test@test.com")).ReturnsAsync(user);
        _repoMock.Setup(r => r.ConnectUserAsync("test@test.com", "pass")).ReturnsAsync(true);

        var result = await _service.ConnectUserAsync("test@test.com", "pass");

        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task ConnectUserAsync_InvalidCredentials_ReturnsNull()
    {
        var user = new User { Email = "test@test.com", Id = "u1" };
        _repoMock.Setup(r => r.GetAsync("test@test.com")).ReturnsAsync(user);
        _repoMock.Setup(r => r.ConnectUserAsync("test@test.com", "pass")).ReturnsAsync(false);

        var result = await _service.ConnectUserAsync("test@test.com", "pass");

        Assert.Null(result);
    }

    [Fact]
    public async Task ConnectUserAsync_UserNotFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetAsync("unknown@test.com")).ReturnsAsync((User?)null);

        var result = await _service.ConnectUserAsync("unknown@test.com", "pass");

        Assert.Null(result);
        _repoMock.Verify(r => r.ConnectUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task CreateUserAsync_DelegatesToRepository()
    {
        await _service.CreateUserAsync("John", "Doe", "john@test.com", "hashed");

        _repoMock.Verify(r => r.AddUserAsync("John", "Doe", "john@test.com", "hashed"), Times.Once);
    }

    [Fact]
    public async Task UpdatePasswordAsync_DelegatesToRepository()
    {
        await _service.UpdatePasswordAsync("test@test.com", "newpass");

        _repoMock.Verify(r => r.UpdatePasswordAsync("test@test.com", "newpass"), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_UserExists_RemovesUser()
    {
        var user = new User { Email = "test@test.com", Id = "u1" };
        _repoMock.Setup(r => r.GetAsync("test@test.com")).ReturnsAsync(user);

        await _service.DeleteUserAsync("test@test.com");

        _repoMock.Verify(r => r.RemoveUserAsync("u1"), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_UserNotFound_DoesNothing()
    {
        _repoMock.Setup(r => r.GetAsync("unknown@test.com")).ReturnsAsync((User?)null);

        await _service.DeleteUserAsync("unknown@test.com");

        _repoMock.Verify(r => r.RemoveUserAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUserAsync_UserHasNullId_DoesNothing()
    {
        var user = new User { Email = "test@test.com", Id = null };
        _repoMock.Setup(r => r.GetAsync("test@test.com")).ReturnsAsync(user);

        await _service.DeleteUserAsync("test@test.com");

        _repoMock.Verify(r => r.RemoveUserAsync(It.IsAny<string>()), Times.Never);
    }
}
