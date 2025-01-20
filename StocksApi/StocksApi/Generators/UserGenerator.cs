using Library.Models;
using Library.Models.Shares;
using Library.Models.Users;
using Library.Models.Users.Notifications;

namespace StocksApi.Generators
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
				WatchingStocksByListName = new Dictionary<string, Dictionary<string, WatchingStock>>(),
				StockNotifications = new List<StockNotification>(),
				UserStockNotesBySymbol = new Dictionary<string, List<UserStockNote>>()
			};

			return user;
		}
	}
}