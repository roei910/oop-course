using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.Finance127;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class YahooFinance127 : IStockAnalysisApi
{
    private readonly YahooFinanceHttpClient _httpClient;
    private readonly ILogger<YahooFinance127> _logger;

    public YahooFinance127(IHttpClientFactory httpClientFactory, ILogger<YahooFinance127> logger)
    {
        _httpClient = new YahooFinanceHttpClient(httpClientFactory.CreateClient("YahooFinance127")!);
        _logger = logger;
    }

    public async Task<StockAnalysis?> GetStockAnalysisAsync(string symbol)
    {
        var response = await _httpClient.GetAsync<Finance127AnalysisResponse>(
            $"finance-analytics/{symbol}");

        if (response is null)
            return null;

        return StockMapper.MapToStockAnalysis(symbol, response);
    }

    public async Task<List<StockAnalysis>> GetStocksAnalysisAsync(string[] symbols)
    {
        var tasks = symbols.Select(GetStockAnalysisAsync);
        var results = await Task.WhenAll(tasks);

        return results
            .Where(r => r is not null)
            .Select(r => r!)
            .ToList();
    }
}
