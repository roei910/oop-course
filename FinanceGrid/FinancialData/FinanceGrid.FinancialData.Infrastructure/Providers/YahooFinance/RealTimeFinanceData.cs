using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.RealTimeFinanceData;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class RealTimeFinanceData : IRealTimeFinanceData
{
    private readonly YahooFinanceHttpClient _httpClient;
    private readonly ILogger<RealTimeFinanceData> _logger;

    public RealTimeFinanceData(IHttpClientFactory httpClientFactory, ILogger<RealTimeFinanceData> logger)
    {
        _httpClient = new YahooFinanceHttpClient(httpClientFactory.CreateClient("RealTimeFinanceData")!);
        _logger = logger;
    }

    public async Task<MarketTrend?> GetMarketTrendAsync(string trendType)
    {
        var response = await _httpClient.GetAsync<RealTimeFinanceBasicResponse>(
            "market-trends",
            ("trend_type", trendType));

        if (response?.Data is null)
            return null;

        return TrendMapper.MapToMarketTrend(trendType, response.Data);
    }
}
