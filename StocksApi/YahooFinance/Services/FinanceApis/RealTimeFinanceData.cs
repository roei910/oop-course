using Library.Models;
using Microsoft.Extensions.Logging;
using YahooFinance.Factories;
using YahooFinance.Interfaces;
using YahooFinance.Models.RealTimeFinanceData;

namespace YahooFinance.Services.FinanceApis
{
	public class RealTimeFinanceData : IRealTimeFinanceData
	{
        private readonly IWebApi _webApi;
        private readonly ILogger<Stock> _logger;

        public RealTimeFinanceData(
            WebApiFactory webApiFactory,
            ILogger<Stock> logger)
        {
            _webApi = webApiFactory.Generate(ConfigurationKeys.RealTimeFinanceDataSection);
            _logger = logger;
        }

        public async Task<MarketTrendsResponse?> GetMarketTrendAsync(string trendType)
        {
            var endPoint = "market-trends";
            var queryParams = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("trend_type", trendType)
            };

            try
            {
                var response = await _webApi
                .GetResponseAsync<RealTimeFinanceBasicResponse>(endPoint, queryParams.ToArray());

                return response!.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while trying to get market trends, trend: {trend}", trendType);

                return null;
            }
        }
    }
}