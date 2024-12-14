namespace Library.Models.Users
{
	public class PasswordUpdateRequest
	{
		public required string Email { get; set; }
		public required string Password { get; set; }
	}
}