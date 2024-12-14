using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StocksApi.Interfaces;
using Library.Models;
using Library.Models.Users;

namespace StocksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IStockRepository _stockRepository;

        public UserController(
            IUserRepository userRepository,
            IStockRepository stockRepository)
        {
            _userRepository = userRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetByEmailAsync(
            [FromQuery] string email)
        {
            var user = await _userRepository.GetAsync(email);

            if (user is null)
                return NotFound("User wasn't found");

            return user;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] string email)
        {
            var user = await _userRepository.GetAsync(email);

            if (user is null)
                return NotFound();

            await _userRepository.RemoveUserAsync(user.Id!);

            return Ok();
        }

        [HttpGet("all")]
        public async Task<List<User>> GetAsync() =>
            await _userRepository.GetAllAsync();

        [HttpPost("connect-user")]
        public async Task<IActionResult> ConnectUserAsync(
            [FromBody] UserCredentials user)
        {
            var foundUser = await _userRepository.GetAsync(user.Email!);

            if(foundUser is null)
                return NotFound("User wasn't found");

            var isAuthenticated = await _userRepository.ConnectUserAsync(user);

            if (!isAuthenticated)
                return Forbid("credentials couldnt match any user in our database");

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAsync(UserDetails userDetails)
        {
            await _userRepository.AddUserAsync(userDetails);

            return Ok("User created succesfully");
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePasswordAsync(PasswordUpdateRequest passwordUpdateRequest)
        {
            var foundUser = await _userRepository.GetAsync(passwordUpdateRequest.Email);

            if (foundUser is null)
                return NotFound();

            await _userRepository.UpdatePasswordAsync(passwordUpdateRequest);

            return Ok("User created succesfully");
        }

        [HttpPost("notification")]
        public async Task<IActionResult> AddStockNotificationAsync(StockNotification stockNotification)
        {
            var foundStock = await _stockRepository.GetStockBySymbolAsync(stockNotification.StockSymbol);
            var foundUser = await _userRepository.GetAsync(stockNotification.UserEmail);

            if (foundStock is null || foundUser is null)
                return NotFound();

            stockNotification.Id = ObjectId.GenerateNewId().ToString();
            stockNotification.StockSymbol = stockNotification.StockSymbol.ToUpper();
            stockNotification.IsTargetBiggerThanOrEqual = stockNotification.TargetPrice > foundStock.Price;
            stockNotification.ShouldBeNotified = false;

            await _userRepository.AddNotificationAsync(stockNotification);
            await _stockRepository.AddNotificationAsync(stockNotification);

            return Ok(new { stockNotification.Id });
        }

        [HttpDelete("notification")]
        public async Task<IActionResult> DeleteNotificationAsync([FromQuery] string email, [FromQuery] string notificationId)
        {
            var user = await _userRepository.GetAsync(email);

            if (user is null)
                return NotFound();

            var stock = user.StockNotifications
                .Where(notification => notification.Id == notificationId)
                .First();

            await _userRepository.RemoveNotificationAsync(user.Id!, notificationId);
            await _stockRepository.RemoveNotificationAsync(stock.StockSymbol, notificationId);

            return Ok();
        }
    }
}