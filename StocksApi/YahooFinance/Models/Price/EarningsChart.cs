namespace YahooFinance.Models.Price
{
    public class EarningsChart
    {
        public List<QuarterlyEarnings>? Quarterly { get; set; }
        public double CurrentQuarterEstimate { get; set; }
        public string? CurrentQuarterEstimateDate { get; set; }
        public int CurrentQuarterEstimateYear { get; set; }
        public List<long>? EarningsDate { get; set; }
    }
}