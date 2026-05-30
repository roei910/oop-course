namespace StocksAbstractions.Models.Stocks
{
    public class StockHistoryEntry
    {
        public string? Id { get; set; }
        public required string StockSymbol { get; set; }
        public required DateOnly Date { get; set; }
        public required double PriceOpen { get; set; }
        public required double PriceClose { get; set; }
        public required double DayLow { get; set; }
        public required double DayHigh { get; set; }
        public required string DayRange { get; set; }
        public required long DayVolume { get; set; }
    }
}
