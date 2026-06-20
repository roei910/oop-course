using Microsoft.AspNetCore.Mvc;
using UsersAbstractions.Dal;
using UsersAbstractions.Repositories;
using StocksAbstractions.Repositories;
using UsersAbstractions.Models.Shares;
using UsersLibrary.Generators;

namespace StocksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShareController: Controller
	{
        private readonly IUserStockWatchesDal _userStockWatchesDal;
        private readonly IUsersDal _usersDal;
        private readonly IUserRepository _userRepository;
        private readonly IStockRepository _stockRepository;

        public ShareController(IUserStockWatchesDal userStockWatchesDal,
            IUsersDal usersDal,
            IUserRepository userRepository,
            IStockRepository stockRepository)
		{
            _userStockWatchesDal = userStockWatchesDal;
            _usersDal = usersDal;
            _userRepository = userRepository;
            _stockRepository = stockRepository;
		}

        [HttpPost]
        public async Task<IActionResult> AddShareAsync([FromBody] SharePurchase sharePurchase)
        {
            sharePurchase.StockSymbol = sharePurchase.StockSymbol.ToUpper();

            var foundUser = await _userRepository.GetAsync(sharePurchase.UserEmail);

            if (foundUser is null)
                return NotFound("User wasn't found");

            var foundStock = await _stockRepository.GetStockBySymbolAsync(sharePurchase.StockSymbol);

            if (foundStock is null)
                return NotFound("Stock wasn't found");

            var share = ShareGenerator.Generate(sharePurchase);

            await _userStockWatchesDal.UpsertWatchAndAddShareAsync(
                sharePurchase.UserEmail,
                sharePurchase.ListName,
                sharePurchase.StockSymbol,
                share.Id,
                share);

            if (!foundUser.WatchListNames.Contains(sharePurchase.ListName))
                await _usersDal.AddWatchListNameAsync(sharePurchase.UserEmail, sharePurchase.ListName);

            return Ok(share);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveShareAsync(
            [FromBody] ShareSale shareSale)
        {
            var found = await _userRepository.GetAsync(shareSale.UserEmail);

            if (found is null)
                return NotFound();

            await _userStockWatchesDal.RemoveShareFromWatchAsync(
                shareSale.UserEmail,
                shareSale.ListName,
                shareSale.StockSymbol,
                shareSale.SharePurchaseGuid);

            return Ok();
        }

        [HttpPost("list")]
        public async Task<IActionResult> AddUserListAsync([FromBody] StockListDetails stockListDetails)
        {
            var foundUser = await _userRepository.GetAsync(stockListDetails.UserEmail);

            if (foundUser is null)
                return NotFound("User wasn't found");

            if (string.IsNullOrWhiteSpace(stockListDetails.ListName))
                return StatusCode(400, "List name cannot contain an empty value");

            if (foundUser.WatchListNames.Contains(stockListDetails.ListName))
                return Conflict("Stock list was found with that name, please enter another name");

            await _usersDal.AddWatchListNameAsync(stockListDetails.UserEmail, stockListDetails.ListName);

            return Ok();
        }

        [HttpDelete("list")]
        public async Task<IActionResult> RemoveUserListAsync([FromBody] StockListDetails stockListDetails)
        {
            var foundUser = await _userRepository.GetAsync(stockListDetails.UserEmail);

            if (foundUser is null)
                return NotFound("User wasn't found");

            if (!foundUser.WatchListNames.Contains(stockListDetails.ListName))
                return NotFound("Stock list wasn't found");

            await _userStockWatchesDal.DeleteWatchesByListAsync(stockListDetails.UserEmail, stockListDetails.ListName);
            await _usersDal.RemoveWatchListNameAsync(stockListDetails.UserEmail, stockListDetails.ListName);

            return Ok();
        }

        [HttpPost("watching-stock")]
        public async Task<IActionResult> AddWatchingStockAsync(
            [FromBody] WatchingStockAction watchingStockAction)
        {
            watchingStockAction.StockSymbol = watchingStockAction.StockSymbol.ToUpper();

            var found = await _userRepository.GetAsync(watchingStockAction.Email);

            if (found is null)
                return NotFound();

            var foundStock = await _stockRepository.GetStockBySymbolAsync(watchingStockAction.StockSymbol);

            if (foundStock is null)
                return NotFound("Stock not found");

            var watch = new UserStockWatch
            {
                UserEmail = watchingStockAction.Email,
                ListName = watchingStockAction.ListName,
                StockSymbol = watchingStockAction.StockSymbol,
                Note = null,
                PurchaseGuidToShares = new Dictionary<string, Share>()
            };

            await _userStockWatchesDal.InsertWatchAsync(watch);

            return Ok();
        }

        [HttpDelete("watching-stock")]
        public async Task<IActionResult> RemoveWatchingStockAsync(
            [FromBody] WatchingStockAction watchingStockAction)
        {
            var found = await _userRepository.GetAsync(watchingStockAction.Email);

            if (found is null)
                return NotFound();

            await _userStockWatchesDal.DeleteWatchAsync(
                watchingStockAction.Email,
                watchingStockAction.ListName,
                watchingStockAction.StockSymbol);

            return Ok();
        }

        [HttpPatch("watching-stock-note")]
        public async Task<IActionResult> UpdateWatchingStockNoteAsync(
            [FromBody] WatchingStockAction watchingStockAction)
        {
            if (watchingStockAction.Note is null)
                return BadRequest("Missing note field");

            await _userStockWatchesDal.UpdateWatchNoteAsync(
                watchingStockAction.Email,
                watchingStockAction.ListName,
                watchingStockAction.StockSymbol,
                watchingStockAction.Note);

            return Ok();
        }
    }
}
