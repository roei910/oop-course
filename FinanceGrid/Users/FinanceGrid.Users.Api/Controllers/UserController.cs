using FinanceGrid.Users.Application.Interfaces;
using FinanceGrid.Users.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinanceGrid.Users.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<User>> GetByEmailAsync([FromQuery] string email)
    {
        var user = await _userService.GetByEmailAsync(email);
        if (user is null) return NotFound("User wasn't found");
        return Ok(user);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromQuery] string email)
    {
        await _userService.DeleteUserAsync(email);
        return Ok();
    }

    [HttpGet("all")]
    public async Task<List<User>> GetAsync() =>
        await _userService.GetAllAsync();

    [HttpPost("connect-user")]
    public async Task<IActionResult> ConnectUserAsync([FromBody] UserCredentials credentials)
    {
        var user = await _userService.ConnectUserAsync(credentials.Email, credentials.Password);
        if (user is null) return NotFound("Invalid credentials");
        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateAsync([FromBody] UserDetails details)
    {
        await _userService.CreateUserAsync(details.FirstName, details.LastName, details.Email, details.Password);
        return Ok("User created successfully");
    }

    [HttpPost("update-password")]
    public async Task<IActionResult> UpdatePasswordAsync([FromBody] PasswordUpdateRequest request)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
        if (user is null) return NotFound();
        await _userService.UpdatePasswordAsync(request.Email, request.Password);
        return Ok("User password updated successfully");
    }
}

public class UserCredentials
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserDetails
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class PasswordUpdateRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
