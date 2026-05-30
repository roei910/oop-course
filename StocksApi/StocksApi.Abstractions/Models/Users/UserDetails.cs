using System.ComponentModel.DataAnnotations;

namespace StocksApi.Abstractions.Models.Users
{
	public class UserDetails
	{
        [Required]
        [StringLength(100)]
        public required string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public required string LastName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [StringLength(256, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}
