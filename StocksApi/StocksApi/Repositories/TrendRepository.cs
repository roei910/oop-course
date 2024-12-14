using Library.Interfaces;
using Library.Models;
using Library.Models.MarketTrends;
using StocksApi.Interfaces;
using YahooFinance.Interfaces;

namespace StocksApi.Repositories
{
	public class TrendRepository : ITrendRepository
	{
        private readonly ITrendsDal _trendsDal;
        private readonly IRealTimeFinanceData _realTimeFinancialData;
        private readonly ILogger<MarketTrend> _logger;
        private readonly List<string> _trendTypes;

        public TrendRepository(
			ITrendsDal trendsDal,
			IRealTimeFinanceData realTimeFinanceData,
			ILogger<MarketTrend> logger,
			IAppConfiguration appConfiguration)
		{
			_trendsDal = trendsDal;
			_realTimeFinancialData = realTimeFinanceData;
			_logger = logger;
            _trendTypes = appConfiguration.Get<List<string>>(
                ConfigurationKeys.RealTimeFinanceDataSection,
                ConfigurationKeys.RealTimeFinanceMarketTrends);
        }

        public async Task<List<MarketTrend>> GetMarketTrendsAsync()
        {
			var tasks = _trendTypes.Select(GetTrendAsync).ToList();

			await Task.WhenAll(tasks);

			var trends = tasks
				.Where(task => task.Result is not null)
				.Select(task => task.Result!)
				.ToList();

			return trends;
        }

        public async Task<MarketTrend?> GetTrendAsync(string trendType)
		{
			var trend = await _trendsDal.FindOneAsync(trendType);

			if(trend is not null)
			{
				var now = DateTime.UtcNow;
				var passedTimeSinceUpdate = now - trend.LastUpdatedTime;

				if (passedTimeSinceUpdate.Days < 2)
					return trend;
			}

			var marketTrend = await _realTimeFinancialData.GetMarketTrendAsync(trendType);

			if (marketTrend is null)
				return trend;

			var updatedTrend = Generators.TrendGenerator.Generate(trendType, marketTrend);

			await _trendsDal.AddOrUpdateOneAsync(trendType, updatedTrend);

			_logger.LogInformation("Updated market trend, {trendName}", trendType);

			return updatedTrend;
		}
	}
}