using System.ComponentModel.DataAnnotations;

namespace StocksApi.Abstractions.Models.Users.StockNotes
{
	public class UserStockNoteUpdateRequest
	{
        [Required]
        public required string Id { get; set; }
        [Required]
        public required string UserEmail { get; set; }
        [Required]
        public required string StockSymbol { get; set; }
        [Required]
        public required string Note { get; set; }
    }
}
