namespace Library.Models
{
    public class Variables
	{
		public required string PASSWORD_SALT { get; set; }
        public int OPEN_MARKET_DAY { get; set; }
        public int CLOSED_MARKET_DAY { get; set; }
        public int OPEN_MARKET_HOURS { get; set; }
        public int CLOSED_MARKET_HOURS { get; set; }
        public int OPEN_MARKET_MINUTES { get; set; }
        public int MINUTE_INTERVAL_BETWEEN_UPDATE { get; set; }
        public int MAX_ALLOWED_ANALYSIS_PER_DAY { get; set; }
    }
}