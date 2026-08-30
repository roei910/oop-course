using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance.Models.Finance15;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class YahooFinance15 : IYahooFinance, IYahooFinanceBulk
{
    private readonly YahooFinanceHttpClient _httpClient;
    private readonly ILogger<YahooFinance15> _logger;

    public YahooFinance15(IHttpClientFactory httpClientFactory, ILogger<YahooFinance15> logger)
    {
        _httpClient = new YahooFinanceHttpClient(httpClientFactory.CreateClient("YahooFinance15")!);
        _logger = logger;
    }

    public async Task<List<StockSearchResult>> FindStockAsync(string searchTerm)
    {
        var response = await _httpClient.GetAsync<BasicResponse<StockSearchResult>>(
            "markets/search",
            ("search", searchTerm));

        return response?.Body ?? [];
    }

    public async Task<Stock?> GetStockAsync(string symbol)
    {
        var stocks = await GetStocksAsync(new[] { symbol });
        return stocks.FirstOrDefault();
    }

    public async Task<List<Stock>> GetStocksAsync(string[] symbols)
    {
        var response = await _httpClient.GetAsync<BasicResponse<PriceResponse>>(
            "markets/stock/quotes",
            ("ticker", string.Join(",", symbols)));

        return response?.Body
            .Select(StockMapper.MapToStock)
            .ToList() ?? [];
    }
}
