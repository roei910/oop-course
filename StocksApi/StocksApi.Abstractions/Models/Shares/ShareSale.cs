using System.ComponentModel.DataAnnotations;

namespace StocksApi.Abstractions.Models.Shares
{
    public class ShareSale
	{
        [Required]
        public required string UserEmail { get; set; }
        [Required]
        public required string ListName { get; set; }
        [Required]
        public required string StockSymbol { get; set; }
        [Required]
        public required string SharePurchaseGuid { get; set; }
	}
}
