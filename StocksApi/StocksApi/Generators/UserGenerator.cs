﻿using Library.Models;
using Library.Models.Shares;
using Library.Models.Users;

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
				StockNotifications = new List<StockNotification>()
			};

			return user;
		}
	}
}