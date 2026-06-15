using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.Finance1;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class YahooFinance1 : IYahooFinance, IYahooFinanceBulk
{
    private readonly IWebApi _webApi;
    private readonly ILogger<YahooFinance1> _logger;

    public YahooFinance1(WebApiFactory webApiFactory, ILogger<YahooFinance1> logger)
    {
        _webApi = webApiFactory.Generate("YahooFinance1");
        _logger = logger;
    }

    public async Task<List<StockSearchResult>> FindStockAsync(string searchTerm)
    {
        var endPoint = "auto-complete";
        var queryParams = new List<KeyValuePair<string, string>>
        {
            new("region", "US"),
            new("q", searchTerm)
        };

        try
        {
            var response = await _webApi
                .GetResponseAsync<Finance1SearchResponse>(endPoint, queryParams.ToArray());

            if (response is null)
                return [];

            return response.Quotes
                .Select(StockMapper.MapToStockSearchResult)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to find stocks from YahooFinance1, search term {SearchTerm}", searchTerm);
            return [];
        }
    }

    public async Task<Stock?> GetStockAsync(string symbol)
    {
        var stocks = await GetStocksAsync(new[] { symbol });
        return stocks.FirstOrDefault();
    }

    public async Task<List<Stock>> GetStocksAsync(string[] symbols)
    {
        try
        {
            var endPoint = "market/v2/get-quotes";
            var queryParams = new List<KeyValuePair<string, string>>
            {
                new("region", "US"),
                new("symbols", string.Join(",", symbols))
            };

            var response = await _webApi
                .GetResponseAsync<Finance1BulkResponse>(endPoint, queryParams.ToArray());

            return response?.QuoteResponse.Result
                .Select(StockMapper.MapToStock)
                .ToList() ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to get stocks from YahooFinance1, {Count} symbols", symbols.Length);
            return [];
        }
    }
}
