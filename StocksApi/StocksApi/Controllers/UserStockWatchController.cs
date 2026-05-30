using Microsoft.AspNetCore.Mvc;
using UsersAbstractions.Dal;
using UsersAbstractions.Models.Shares;

namespace StocksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserStockWatchController : Controller
    {
        private readonly IUserStockWatchesDal _userStockWatchesDal;

        public UserStockWatchController(IUserStockWatchesDal userStockWatchesDal)
        {
            _userStockWatchesDal = userStockWatchesDal;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserStockWatch>>> GetByEmailAsync(
            [FromQuery] string email, [FromQuery] string? listName)
        {
            List<UserStockWatch> watches;

            if (!string.IsNullOrWhiteSpace(listName))
                watches = await _userStockWatchesDal.GetWatchesByEmailAndListAsync(email, listName);
            else
                watches = await _userStockWatchesDal.GetWatchesByEmailAsync(email);

            return Ok(watches);
        }
    }
}
