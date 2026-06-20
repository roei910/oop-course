using Microsoft.AspNetCore.Mvc;
using UsersAbstractions.Repositories;
using UsersAbstractions.Models;
using UsersAbstractions.Models.Users;

namespace UsersService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetByEmailAsync([FromQuery] string email)
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
        public async Task<IActionResult> ConnectUserAsync([FromBody] UserCredentials userCredentials)
        {
            var foundUser = await _userRepository.GetAsync(userCredentials.Email!);

            if (foundUser is null)
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
    }
}
