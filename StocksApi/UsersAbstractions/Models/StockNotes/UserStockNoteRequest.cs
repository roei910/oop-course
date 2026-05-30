using System.ComponentModel.DataAnnotations;

namespace UsersAbstractions.Models.StockNotes
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
