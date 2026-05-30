using System.ComponentModel.DataAnnotations;

namespace StocksApi.Abstractions.Models.Shares
{
    public class WatchingStockAction
	{
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string ListName { get; set; }
        [Required]
        public required string StockSymbol { get; set; }
		public string? Note { get; set; }
	}
}
