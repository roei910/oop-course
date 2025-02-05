using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StocksApi.Interfaces;
using Library.Models;
using Library.Models.Users;
using Library.Models.Users.StockNotes;
using Library.Models.ServerResponse;
using Library.Models.Users.Notifications;
using StocksApi.Generators;

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

            return Ok(user);
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
            [FromBody] UserCredentials userCredentials)
        {
            var foundUser = await _userRepository.GetAsync(userCredentials.Email!);

            if(foundUser is null)
                return NotFound("User wasn't found");

            var isAuthenticated = await _userRepository.ConnectUserAsync(userCredentials);

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

            return Ok("User password updated succesfully");
        }

        [HttpPost("notification")]
        public async Task<IActionResult> AddStockNotificationAsync(StockNotificationRequest request)
        {
            var foundStock = await _stockRepository.GetStockBySymbolAsync(request.StockSymbol);
            var foundUser = await _userRepository.GetAsync(request.UserEmail);

            if (foundStock is null || foundUser is null)
                return NotFound();

            var stockNotification = StockNotificationGenerator.Generate(request, foundStock.Price);

            await _userRepository.AddNotificationAsync(stockNotification);
            await _stockRepository.AddNotificationAsync(stockNotification);

            var response = new ObjectIdResponse
            {
                Id = stockNotification.Id
            };

            return Ok(response);
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

        [HttpPost("stockNote")]
        public async Task<IActionResult> AddUserStockNoteAsync([FromBody] UserStockNoteRequest userStockNoteRequest)
        {
            userStockNoteRequest.StockSymbol = userStockNoteRequest.StockSymbol.ToUpper();

            var user = await _userRepository.GetAsync(userStockNoteRequest.UserEmail);
            var stock = await _stockRepository.GetStockBySymbolAsync(userStockNoteRequest.StockSymbol);

            if (user is null || stock is null)
                return NotFound();

            var stockNoteId = await _userRepository.AddStockNoteAsync(userStockNoteRequest);

            var response = new ObjectIdResponse
            {
                Id = stockNoteId
            };

            return Ok(response);
        }

        [HttpPatch("stockNote")]
        public async Task<IActionResult> UpdateUserStockNoteAsync([FromBody] UserStockNoteUpdateRequest noteUpdateRequest)
        {
            var user = await _userRepository.GetAsync(noteUpdateRequest.UserEmail);

            if (user is null)
                return NotFound();

            await _userRepository.UpdateStockNoteAsync(noteUpdateRequest);

            return Ok();
        }

        [HttpDelete("stockNote")]
        public async Task<IActionResult> RemoveUserStockNoteAsync(
            [FromQuery] string userEmail,
            [FromQuery] string stockSymbol,
            [FromQuery] string noteId)
        {
            //TODO: working with sessions??
            var user = await _userRepository.GetAsync(userEmail);
            var stock = await _stockRepository.GetStockBySymbolAsync(stockSymbol);

            if (user is null || stock is null)
                return NotFound();

            await _userRepository.RemoveStockNoteAsync(userEmail, stockSymbol.ToUpper(), noteId);

            return Ok();
        }
    }
}