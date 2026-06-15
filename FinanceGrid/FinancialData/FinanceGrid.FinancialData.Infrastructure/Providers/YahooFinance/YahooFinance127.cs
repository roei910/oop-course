using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.Finance127;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class YahooFinance127 : IStockAnalysisApi
{
    private readonly IWebApi _webApi;
    private readonly ILogger<YahooFinance127> _logger;

    public YahooFinance127(WebApiFactory webApiFactory, ILogger<YahooFinance127> logger)
    {
        _webApi = webApiFactory.Generate("YahooFinance127");
        _logger = logger;
    }

    public async Task<StockAnalysis?> GetStockAnalysisAsync(string symbol)
    {
        try
        {
            var endPoint = $"finance-analytics/{symbol}";

            var response = await _webApi.GetResponseAsync<Finance127AnalysisResponse>(endPoint);

            if (response is null)
                return null;

            return StockMapper.MapToStockAnalysis(symbol, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to get stock analysis from YahooFinance127, symbol {Symbol}", symbol);
            return null;
        }
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
