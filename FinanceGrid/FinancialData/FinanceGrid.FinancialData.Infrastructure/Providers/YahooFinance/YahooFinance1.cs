using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.Finance1;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class YahooFinance1 : IYahooFinance, IYahooFinanceBulk
{
    private readonly YahooFinanceHttpClient _httpClient;
    private readonly ILogger<YahooFinance1> _logger;

    public YahooFinance1(IHttpClientFactory httpClientFactory, ILogger<YahooFinance1> logger)
    {
        _httpClient = new YahooFinanceHttpClient(httpClientFactory.CreateClient("YahooFinance1")!);
        _logger = logger;
    }

    public async Task<List<StockSearchResult>> FindStockAsync(string searchTerm)
    {
        var response = await _httpClient.GetAsync<Finance1SearchResponse>(
            "auto-complete",
            ("region", "US"),
            ("q", searchTerm));

        if (response is null)
            return [];

        return response.Quotes
            .Select(StockMapper.MapToStockSearchResult)
            .ToList();
    }

    public async Task<Stock?> GetStockAsync(string symbol)
    {
        var stocks = await GetStocksAsync(new[] { symbol });
        return stocks.FirstOrDefault();
    }

    public async Task<List<Stock>> GetStocksAsync(string[] symbols)
    {
        var response = await _httpClient.GetAsync<Finance1BulkResponse>(
            "market/v2/get-quotes",
            ("region", "US"),
            ("symbols", string.Join(",", symbols)));

        return response?.QuoteResponse.Result
            .Select(StockMapper.MapToStock)
            .ToList() ?? [];
    }
}
