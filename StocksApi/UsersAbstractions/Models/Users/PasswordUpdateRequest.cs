using System.ComponentModel.DataAnnotations;

namespace UsersAbstractions.Models.Users
{
	public class PasswordUpdateRequest
	{
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
	}
}
