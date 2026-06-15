using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.RealTimeFinanceData;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class RealTimeFinanceData : IRealTimeFinanceData
{
    private readonly IWebApi _webApi;
    private readonly ILogger<RealTimeFinanceData> _logger;

    public RealTimeFinanceData(WebApiFactory webApiFactory, ILogger<RealTimeFinanceData> logger)
    {
        _webApi = webApiFactory.Generate("RealTimeFinanceData");
        _logger = logger;
    }

    public async Task<MarketTrend?> GetMarketTrendAsync(string trendType)
    {
        var endPoint = "market-trends";
        var queryParams = new List<KeyValuePair<string, string>>
        {
            new("trend_type", trendType)
        };

        try
        {
            var response = await _webApi
                .GetResponseAsync<RealTimeFinanceBasicResponse>(endPoint, queryParams.ToArray());

            if (response?.Data is null)
                return null;

            return TrendMapper.MapToMarketTrend(trendType, response.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to get market trends, trend: {Trend}", trendType);
            return null;
        }
    }
}
