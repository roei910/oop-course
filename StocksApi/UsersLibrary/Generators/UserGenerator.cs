using UsersAbstractions.Models;
using UsersAbstractions.Models.Users;
using UsersAbstractions.Models.Notifications;
using UsersAbstractions.Models.StockNotes;

namespace UsersLibrary.Generators
{
    public static class UserGenerator
	{
		public static User Generate(UserDetails userDetails)
		{
			var user = new User
			{
				Email = userDetails.Email,
				Password = userDetails.Password,
				FirstName = userDetails.FirstName,
				LastName = userDetails.LastName,
				WatchListNames = new List<string>(),
				StockNotifications = new List<StockNotification>(),
				UserStockNotesBySymbol = new Dictionary<string, List<UserStockNote>>()
			};

			return user;
		}
	}
}
