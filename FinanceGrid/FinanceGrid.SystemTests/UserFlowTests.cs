using FinanceGrid.SystemTests.Dtos;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinanceGrid.SystemTests;

[Collection("DockerCompose")]
[Trait("Category", "System")]
public class UserFlowTests
{
    private readonly DockerComposeFixture _fixture;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public UserFlowTests(DockerComposeFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Register_Then_ConnectUser_Flow_Succeeds()
    {
        RequireServices();
        using var http = new HttpClient { BaseAddress = new Uri(DockerComposeFixture.UsersBaseUrl) };

        var email = $"sys-{Guid.NewGuid():N}@example.com";
        var password = "SysPass1!";

        var register = new UserDetails
        {
            FirstName = "Sys",
            LastName = "Test",
            Email = email,
            Password = password
        };
        var regResp = await http.PostAsJsonAsync("/api/user/register", register);
        Assert.Equal(HttpStatusCode.OK, regResp.StatusCode);

        var login = new UserCredentials { Email = email, Password = password };
        var loginResp = await http.PostAsJsonAsync("/api/user/connect-user", login);
        Assert.Equal(HttpStatusCode.OK, loginResp.StatusCode);
    }

    [Fact]
    public async Task GetByEmail_ForExistingUser_ReturnsUser()
    {
        RequireServices();
        using var http = new HttpClient { BaseAddress = new Uri(DockerComposeFixture.UsersBaseUrl) };

        // Register a user first
        var email = $"sys-{Guid.NewGuid():N}@example.com";
        await http.PostAsJsonAsync("/api/user/register", new UserDetails
        {
            FirstName = "Sys",
            LastName = "Test",
            Email = email,
            Password = "SysPass1!"
        });

        var resp = await http.GetAsync($"/api/user?email={email}");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var user = await resp.Content.ReadFromJsonAsync<User>(JsonOptions);
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public async Task Delete_ExistingUser_Flow_Succeeds()
    {
        RequireServices();
        using var http = new HttpClient { BaseAddress = new Uri(DockerComposeFixture.UsersBaseUrl) };

        var email = $"sys-{Guid.NewGuid():N}@example.com";
        await http.PostAsJsonAsync("/api/user/register", new UserDetails
        {
            FirstName = "Del",
            LastName = "User",
            Email = email,
            Password = "DelPass1!"
        });

        var delResp = await http.DeleteAsync($"/api/user?email={email}");
        Assert.Equal(HttpStatusCode.OK, delResp.StatusCode);

        var check = await http.GetAsync($"/api/user?email={email}");
        Assert.Equal(HttpStatusCode.NotFound, check.StatusCode);
    }

    [Fact]
    public async Task UpdatePassword_Then_ConnectUser_Flow_Succeeds()
    {
        RequireServices();
        using var http = new HttpClient { BaseAddress = new Uri(DockerComposeFixture.UsersBaseUrl) };

        var email = $"sys-{Guid.NewGuid():N}@example.com";
        var oldPassword = "OldPass1!";
        var newPassword = "NewPass1!";

        await http.PostAsJsonAsync("/api/user/register", new UserDetails
        {
            FirstName = "Pwd",
            LastName = "Chg",
            Email = email,
            Password = oldPassword
        });

        var update = new PasswordUpdateRequest { Email = email, Password = newPassword };
        var updateResp = await http.PostAsJsonAsync("/api/user/update-password", update);
        Assert.Equal(HttpStatusCode.OK, updateResp.StatusCode);

        var loginResp = await http.PostAsJsonAsync("/api/user/connect-user",
            new UserCredentials { Email = email, Password = newPassword });
        Assert.Equal(HttpStatusCode.OK, loginResp.StatusCode);
    }

    private void RequireServices()
    {
        if (!_fixture.ServicesAreRunning)
        {
            Assert.Fail("Docker Compose services are not running. " +
                "Set MANAGE_DOCKER_COMPOSE=true or start the stack manually.");
        }
    }
}
