namespace Library.Models.Stocks
{
    public class StockAnalysis
    {
        public required string Symbol { get; set; }
        public DateTime UpdatedTime { get; set; }
        public double TargetHighPrice { get; set; }
        public double TargetLowPrice { get; set; }
        public double TargetMeanPrice { get; set; }
        public double TargetMedianPrice { get; set; }
    }
}