using System.ComponentModel.DataAnnotations;

namespace UsersAbstractions.Models.Users
{
	public class UserCredentials
	{
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
	}
}
