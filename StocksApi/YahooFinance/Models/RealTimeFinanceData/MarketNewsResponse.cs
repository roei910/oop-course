namespace Library.Models.Stocks
{
    public class MarketNewsResponse
    {
        public string? Article_Title { get; set; }
        public string? Article_Url { get; set; }
        public string? Article_Photo_Url { get; set; }
        public string? Source { get; set; }
        public string? Post_Time_Utc { get; set; }
        public List<MarketTrendResponse>? Stocks_In_News { get; set; }
    }
}