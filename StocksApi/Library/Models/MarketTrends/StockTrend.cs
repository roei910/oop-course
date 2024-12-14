namespace Library.Models.MarketTrends
{
    public class StockTrend
    {
        public string? Symbol { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public double? Change { get; set; }
        public double? ChangePercent { get; set; }
        public double? PreviousClose { get; set; }
        public double? PreOrPostMarket { get; set; }
        public double? PreOrPostMarketChange { get; set; }
        public double? PreOrPostMarketChangePercent { get; set; }
        public string? LastUpdateUtc { get; set; }
        public string? Currency { get; set; }
        public string? Exchange { get; set; }
        public string? ExchangeOpen { get; set; }
        public string? ExchangeClose { get; set; }
        public string? Timezone { get; set; }
        public string? CountryCode { get; set; }
    }
}