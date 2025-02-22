namespace Library.Models
{
    public class StockHistory
    {
        public required DateOnly Date { get; set; }
        public required double PriceOpen { get; set; }
        public required double PriceClose { get; set; }
        public required double DayLow { get; set; }
        public required double DayHigh { get; set; }
        public required string DayRange { get; set; }
        public required long DayVolume { get; set; }
    }
}