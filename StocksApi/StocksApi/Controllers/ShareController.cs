using Library.Models.Shares;
using Microsoft.AspNetCore.Mvc;
using StocksApi.Interfaces;

namespace StocksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShareController: Controller
	{
        private readonly IUserRepository _userRepository;
        private readonly IStockRepository _stockRepository;

        public ShareController(IUserRepository userRepository,
            IStockRepository stockRepository)
		{
            _userRepository = userRepository;
            _stockRepository = stockRepository;
		}

        [HttpPost]
        public async Task<IActionResult> AddShareAsync([FromBody] SharePurchase sharePurchase)
        {
            var foundUser = await _userRepository.GetAsync(sharePurchase.UserEmail);

            if (foundUser is null)
                return NotFound("User wasnt found");

            var foundStock = await _stockRepository.GetStockBySymbolAsync(sharePurchase.StockSymbol);

            if (foundStock is null)
                return NotFound("Stock wasnt found");

            var share = await _userRepository.AddShareAsync(foundUser.Id!, sharePurchase);

            return Ok(share);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveShareAsync(
            [FromBody] ShareSale shareSale)
        {
            var found = await _userRepository.GetAsync(shareSale.UserEmail);

            if (found is null)
                return NotFound();

            await _userRepository.RemoveShareAsync(shareSale);

            return Ok();
        }

        [HttpPost("list")]
        public async Task<IActionResult> AddUserListAsync([FromBody] StockListDetails stockListDetails)
        {
            var foundUser = await _userRepository.GetAsync(stockListDetails.UserEmail);

            if (foundUser is null)
                return NotFound("User wasnt found");

            if (string.IsNullOrWhiteSpace(stockListDetails.ListName))
                return StatusCode(400, "List name cannot contain an empty value");

            var listNameExists = foundUser.WatchingStocksByListName
                .ContainsKey(stockListDetails.ListName);

            if (listNameExists)
                return Conflict("Stock list was found with that name, please enter another name");

            await _userRepository.AddStockListAsync(stockListDetails);

            return Ok();
        }

        [HttpDelete("list")]
        public async Task<IActionResult> RemoveUserListAsync([FromBody] StockListDetails stockListDetails)
        {
            var foundUser = await _userRepository.GetAsync(stockListDetails.UserEmail);

            if (foundUser is null)
                return NotFound("User wasnt found");

            var listNameExists = foundUser.WatchingStocksByListName
                .ContainsKey(stockListDetails.ListName);

            if (!listNameExists)
                return NotFound("Stock list wasnt found");

            await _userRepository.RemoveStockListAsync(stockListDetails);

            return Ok();
        }

        [HttpPost("watching-stock")]
        public async Task<IActionResult> AddWatchingStockAsync(
            [FromBody] WatchingStockAction watchingStockAction)
        {
            var found = await _userRepository.GetAsync(watchingStockAction.Email);

            if (found is null)
                return NotFound();

            await _userRepository.AddWatchingStockAsync(found.Id!,
                watchingStockAction.ListName, watchingStockAction.StockSymbol);

            return Ok();
        }

        [HttpDelete("watching-stock")]
        public async Task<IActionResult> RemoveWatchingStockAsync(
            [FromBody] WatchingStockAction watchingStockAction)
        {
            var found = await _userRepository.GetAsync(watchingStockAction.Email);

            if (found is null)
                return NotFound();

            await _userRepository.RemoveWatchingStockAsync(found.Id!,
                watchingStockAction.ListName, watchingStockAction.StockSymbol);

            return Ok();
        }

        [HttpPatch("watching-stock-note")]
        public async Task<IActionResult> UpdateWatchingStockNoteAsync(
            [FromBody] WatchingStockAction watchingStockAction)
        {
            if (watchingStockAction.Note is null)
                return StatusCode(206, "Missing note field");

            await _userRepository.UpdateShareNoteAsync(watchingStockAction);

            return Ok();
        }
    }
}