using FinanceGrid.Users.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceGrid.Users.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShareController : Controller
{
    private readonly IShareService _shareService;
    private readonly IUserService _userService;

    public ShareController(IShareService shareService, IUserService userService)
    {
        _shareService = shareService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> AddShareAsync([FromBody] SharePurchaseRequest request)
    {
        var user = await _userService.GetByEmailAsync(request.UserEmail);
        if (user is null) return NotFound("User wasn't found");

        var share = await _shareService.AddShareAsync(
            request.UserEmail, request.ListName, request.StockSymbol,
            request.PurchasingPrice, request.PurchaseDate, request.Amount);

        if (share is null) return NotFound();
        return Ok(share);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveShareAsync([FromBody] ShareSaleRequest request)
    {
        var user = await _userService.GetByEmailAsync(request.UserEmail);
        if (user is null) return NotFound();

        await _shareService.RemoveShareAsync(
            request.UserEmail, request.ListName, request.StockSymbol, request.SharePurchaseGuid);
        return Ok();
    }

    [HttpPost("list")]
    public async Task<IActionResult> AddUserListAsync([FromBody] StockListRequest request)
    {
        var user = await _userService.GetByEmailAsync(request.UserEmail);
        if (user is null) return NotFound("User wasn't found");
        if (string.IsNullOrWhiteSpace(request.ListName))
            return BadRequest("List name cannot be empty");

        await _shareService.AddUserListAsync(request.UserEmail, request.ListName);
        return Ok();
    }

    [HttpDelete("list")]
    public async Task<IActionResult> RemoveUserListAsync([FromBody] StockListRequest request)
    {
        var user = await _userService.GetByEmailAsync(request.UserEmail);
        if (user is null) return NotFound("User wasn't found");

        await _shareService.RemoveUserListAsync(request.UserEmail, request.ListName);
        return Ok();
    }

    [HttpPost("watching-stock")]
    public async Task<IActionResult> AddWatchingStockAsync([FromBody] WatchingStockRequest request)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
        if (user is null) return NotFound();

        await _shareService.AddWatchingStockAsync(request.Email, request.ListName, request.StockSymbol);
        return Ok();
    }

    [HttpDelete("watching-stock")]
    public async Task<IActionResult> RemoveWatchingStockAsync([FromBody] WatchingStockRequest request)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
        if (user is null) return NotFound();

        await _shareService.RemoveWatchingStockAsync(request.Email, request.ListName, request.StockSymbol);
        return Ok();
    }

    [HttpPatch("watching-stock-note")]
    public async Task<IActionResult> UpdateWatchingStockNoteAsync([FromBody] WatchingStockRequest request)
    {
        await _shareService.UpdateWatchingStockNoteAsync(
            request.Email, request.ListName, request.StockSymbol, request.Note);
        return Ok();
    }
}

public class SharePurchaseRequest
{
    public string UserEmail { get; set; } = string.Empty;
    public string StockSymbol { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public double PurchasingPrice { get; set; }
    public double Amount { get; set; }
    public string ListName { get; set; } = string.Empty;
}

public class ShareSaleRequest
{
    public string UserEmail { get; set; } = string.Empty;
    public string ListName { get; set; } = string.Empty;
    public string StockSymbol { get; set; } = string.Empty;
    public string SharePurchaseGuid { get; set; } = string.Empty;
}

public class StockListRequest
{
    public string UserEmail { get; set; } = string.Empty;
    public string ListName { get; set; } = string.Empty;
}

public class WatchingStockRequest
{
    public string Email { get; set; } = string.Empty;
    public string ListName { get; set; } = string.Empty;
    public string StockSymbol { get; set; } = string.Empty;
    public string? Note { get; set; }
}
