using System.ComponentModel.DataAnnotations;

namespace UsersAbstractions.Models.Shares
{
    public class SharePurchase
	{
        [Required]
        public required string UserEmail { get; set; }
        [Required]
        [StringLength(10)]
        public required string StockSymbol { get; set; }
        [Required]
        public required DateTime PurchaseDate { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public required double PurchasingPrice { get; set; }
        [Required]
        [Range(0.0001, double.MaxValue)]
        public required double Amount { get; set; }
        [Required]
        public required string ListName { get; set; }
    }
}
