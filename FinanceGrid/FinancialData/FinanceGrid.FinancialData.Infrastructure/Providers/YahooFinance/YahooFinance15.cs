using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.Finance15;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class YahooFinance15 : IYahooFinance, IYahooFinanceBulk
{
    private readonly IWebApi _webApi;
    private readonly ILogger<YahooFinance15> _logger;

    public YahooFinance15(WebApiFactory webApiFactory, ILogger<YahooFinance15> logger)
    {
        _webApi = webApiFactory.Generate("YahooFinance15");
        _logger = logger;
    }

    public async Task<List<StockSearchResult>> FindStockAsync(string searchTerm)
    {
        var endPoint = "markets/search";
        var queryParams = new List<KeyValuePair<string, string>>
        {
            new("search", searchTerm)
        };

        try
        {
            var response = await _webApi
                .GetResponseAsync<BasicResponse<StockSearchResult>>(endPoint, queryParams.ToArray());

            return response?.Body ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to find stocks from YahooFinance15, search term {SearchTerm}", searchTerm);
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
            var endPoint = "markets/stock/quotes";
            var queryParams = new List<KeyValuePair<string, string>>
            {
                new("ticker", string.Join(",", symbols))
            };

            var response = await _webApi
                .GetResponseAsync<BasicResponse<PriceResponse>>(endPoint, queryParams.ToArray());

            return response?.Body
                .Select(StockMapper.MapToStock)
                .ToList() ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to get stocks from YahooFinance15, {Count} symbols", symbols.Length);
            return [];
        }
    }
}
