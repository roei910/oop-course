namespace UsersAbstractions.Models.StockNotes
{
	public class UserStockNote
	{
        public required string Id { get; set; }
		public required string Note { get; set; }
        public required DateTime CreationTime { get; set; }
        public required DateTime LastUpdateTime { get; set; }
    }
}
