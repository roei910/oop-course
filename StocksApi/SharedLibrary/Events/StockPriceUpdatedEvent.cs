namespace SharedLibrary.Events
{
    public class StockPriceUpdatedEvent
    {
        public required string Symbol { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public decimal ChangePercent { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
