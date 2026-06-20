using FinanceGrid.Users.Api.Controllers;
using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinanceGrid.Users.IntegrationTests.Controllers;

public class UserControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UserControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidDetails_ReturnsOk()
    {
        // Arrange
        var newUser = new UserDetails
        {
            FirstName = "Alice",
            LastName = "Wonder",
            Email = "alice@example.com",
            Password = "AlicePass1!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/register", newUser);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetByEmail_WithExistingUser_ReturnsOkWithUser()
    {
        // Act
        var response = await _client.GetAsync($"/api/user?email={CustomWebApplicationFactory.SeedUserEmail}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var user = await response.Content.ReadFromJsonAsync<User>(JsonOptions);
        Assert.NotNull(user);
        Assert.Equal(CustomWebApplicationFactory.SeedUserEmail, user.Email);
        Assert.Equal(CustomWebApplicationFactory.SeedUserFirstName, user.FirstName);
    }

    [Fact]
    public async Task GetByEmail_WithMissingUser_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/user?email=missing@example.com");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_WithSeededUsers_ReturnsList()
    {
        // Act
        var response = await _client.GetAsync("/api/user/all");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var users = await response.Content.ReadFromJsonAsync<List<User>>(JsonOptions);
        Assert.NotNull(users);
        Assert.NotEmpty(users);
        Assert.Contains(users, u => u.Email == CustomWebApplicationFactory.SeedUserEmail);
    }

    [Fact]
    public async Task Delete_WithExistingEmail_ReturnsOk()
    {
        // Arrange: Register a new user to delete (don't touch the seeded user)
        var newUser = new UserDetails
        {
            FirstName = "Bob",
            LastName = "Builder",
            Email = "bob.delete@example.com",
            Password = "BobPass1!"
        };
        await _client.PostAsJsonAsync("/api/user/register", newUser);

        // Act
        var response = await _client.DeleteAsync($"/api/user?email={newUser.Email}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify the user is gone
        var checkResponse = await _client.GetAsync($"/api/user?email={newUser.Email}");
        Assert.Equal(HttpStatusCode.NotFound, checkResponse.StatusCode);
    }

    [Fact]
    public async Task ConnectUser_WithValidCredentials_ReturnsOk()
    {
        // Arrange
        var credentials = new UserCredentials
        {
            Email = CustomWebApplicationFactory.SeedUserEmail,
            Password = CustomWebApplicationFactory.SeedUserPassword
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/connect-user", credentials);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ConnectUser_WithInvalidPassword_ReturnsNotFound()
    {
        // Arrange
        var credentials = new UserCredentials
        {
            Email = CustomWebApplicationFactory.SeedUserEmail,
            Password = "WrongPassword"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/connect-user", credentials);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ConnectUser_WithMissingUser_ReturnsNotFound()
    {
        // Arrange
        var credentials = new UserCredentials
        {
            Email = "ghost@example.com",
            Password = "AnyPassword"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/connect-user", credentials);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePassword_WithExistingUser_ReturnsOk()
    {
        // Arrange: Register a new user first
        var newUser = new UserDetails
        {
            FirstName = "Carol",
            LastName = "Danvers",
            Email = "carol@example.com",
            Password = "CarolPass1!"
        };
        await _client.PostAsJsonAsync("/api/user/register", newUser);

        var updateRequest = new PasswordUpdateRequest
        {
            Email = newUser.Email,
            Password = "NewCarolPass1!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/update-password", updateRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePassword_WithMissingUser_ReturnsNotFound()
    {
        // Arrange
        var updateRequest = new PasswordUpdateRequest
        {
            Email = "ghost@example.com",
            Password = "NewPass1!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/update-password", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
