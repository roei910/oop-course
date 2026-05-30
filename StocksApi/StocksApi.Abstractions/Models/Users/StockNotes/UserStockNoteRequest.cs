using System.ComponentModel.DataAnnotations;

namespace StocksApi.Abstractions.Models.Users.StockNotes
{
	public class UserStockNoteRequest
	{
        [Required]
        public required string UserEmail { get; set; }
        [Required]
        public required string StockSymbol { get; set; }
        [Required]
        public required string Note { get; set; }
    }
}
