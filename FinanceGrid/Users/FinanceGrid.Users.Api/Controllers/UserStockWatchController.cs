using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceGrid.Users.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserStockWatchController : Controller
{
    private readonly IUserStockWatchRepository _watchRepository;

    public UserStockWatchController(IUserStockWatchRepository watchRepository)
    {
        _watchRepository = watchRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserStockWatch>>> GetByEmailAsync(
        [FromQuery] string email, [FromQuery] string? listName)
    {
        List<UserStockWatch> watches;

        if (!string.IsNullOrWhiteSpace(listName))
            watches = await _watchRepository.GetWatchesByEmailAndListAsync(email, listName);
        else
            watches = await _watchRepository.GetWatchesByEmailAsync(email);

        return Ok(watches);
    }
}
