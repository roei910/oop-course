namespace UsersAbstractions.Models.Notifications
{
    public class StockNotification
    {
        public string? Id { get; set; }
        public required string StockSymbol { get; set; }
        public required string UserEmail { get; set; }
        public required double TargetPrice { get; set; }
        public bool IsTargetBiggerThanOrEqual { get; set; }
        public bool ShouldBeNotified { get; set; }
    }
}
