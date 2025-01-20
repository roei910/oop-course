namespace Library.Models.Users.StockNotes
{
	public class UserStockNoteUpdateRequest
	{
		public required string Id { get; set; }
		public required string UserEmail { get; set; }
		public required string StockSymbol { get; set; }
        public required string Note { get; set; }
    }
}