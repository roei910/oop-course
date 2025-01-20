namespace Library.Models.Users
{
	public class UserStockNoteRequest
	{
		public required string UserEmail { get; set; }
		public required string StockSymbol { get; set; }
		public required string Note { get; set; }
    }
}