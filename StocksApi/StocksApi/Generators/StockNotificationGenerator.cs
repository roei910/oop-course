using Library.Models.Users.Notifications;
using MongoDB.Bson;

namespace StocksApi.Generators
{
	public class StockNotificationGenerator
	{
		public static StockNotification Generate(StockNotificationRequest request, double stockPrice)
		{
			var stockNotification = new StockNotification
			{
				Id = ObjectId.GenerateNewId().ToString(),
				StockSymbol = request.StockSymbol.ToUpper(),
				UserEmail = request.UserEmail,
				TargetPrice = request.TargetPrice,
				IsTargetBiggerThanOrEqual = request.TargetPrice > stockPrice,
				ShouldBeNotified = false
			};

			return stockNotification;
        }
    }
}