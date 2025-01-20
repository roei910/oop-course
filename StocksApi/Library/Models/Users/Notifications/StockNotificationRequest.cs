namespace Library.Models.Users.Notifications
{
	public class StockNotificationRequest
	{
        public required string StockSymbol { get; set; }
        public required string UserEmail { get; set; }
        public required double TargetPrice { get; set; }
    }
}

