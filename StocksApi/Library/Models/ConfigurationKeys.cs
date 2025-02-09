namespace Library.Models
{
	public class ConfigurationKeys
	{
		public const string SecretsFilePathKey = "SECRETS_FILE_PATH";
        public const string ConnectionStringSection = "ConnectionString";

		public const string AppVariablesSection = "AppVariables";
        public const string DatabaseSettingsSection = "DatabaseSettings";

        public const string Finance15Section = "YahooFinance15";
		public const string Finance127Section = "YahooFinance127";
		public const string Finance1Section = "YahooFinance1";
		public const string RealTimeFinanceDataSection = "RealTimeFinanceData";

		public const string RealTimeFinanceMarketTrends = "MarketTrends";
    }
}