using UsersAbstractions.Models.Notifications;

namespace UsersLibrary.Generators
{
	public class StockNotificationGenerator
	{
		public static StockNotification Generate(StockNotificationRequest request, double stockPrice)
		{
			var stockNotification = new StockNotification
			{
				Id = Guid.NewGuid().ToString(),
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
