using System.ComponentModel.DataAnnotations;

namespace StocksApi.Abstractions.Models.Users.Notifications
{
	public class StockNotificationRequest
	{
        [Required]
        public required string StockSymbol { get; set; }
        [Required]
        public required string UserEmail { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public required double TargetPrice { get; set; }
    }
}

