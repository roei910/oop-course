using Library.Models;
using Library.Models.SearchResults;
using Microsoft.Extensions.Logging;
using YahooFinance.Factories;
using YahooFinance.Generators;
using YahooFinance.Interfaces;
using YahooFinance.Models;
using YahooFinance.Models.YahooFinance1;

namespace YahooFinance.Services.FinanceApis
{
    public class YahooFinance1 : IYahooFinance, IYahooFinanceBulk
    {
        private readonly IWebApi _webApi;
        private readonly ILogger<Stock> _logger;

        public YahooFinance1(WebApiFactory webApiFactory, ILogger<Stock> logger)
        {
            _webApi = webApiFactory.Generate(ConfigurationKeys.Finance1Section);
            _logger = logger;
        }

        public async Task<List<StockSearchResult>> FindStockAsync(string searchTerm)
        {
            var endPoint = "auto-complete";
            var queryParams = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("region", "US"),
                new KeyValuePair<string, string>("q", searchTerm)
            };

            try
            {
                var response = await _webApi
                .GetResponseAsync<Finance1SearchResponse>(endPoint, queryParams.ToArray());

                if (response is null)
                    return new List<StockSearchResult>();

                var stockSearchInformation = response.Quotes
                    .Select(StockSearchInformationGenerator.Generate)
                    .ToList();

                return stockSearchInformation;
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, "error while trying to find stocks from yahoo, search term {searchTerm}", searchTerm);

                return new List<StockSearchResult>();
            }
        }

        public async Task<PriceResponse?> GetStockAsync(string symbol)
        {
            var stocks = await GetStocksAsync(symbol);

            return stocks.FirstOrDefault();
        }

        public async Task<List<PriceResponse>> GetStocksAsync(params string[] symbols)
        {
            try
            {
                var endPoint = "market/v2/get-quotes";
                var queryParams = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("region", "US"),
                    new KeyValuePair<string, string>("symbols", string.Join(",", symbols))
                };

                var response = await _webApi
                    .GetResponseAsync<Finance1BulkResponse>(endPoint, queryParams.ToArray());

                return response?.QuoteResponse?.Result ?? new List<PriceResponse>();
            }
            catch (Exception)
            {
                _logger.LogError("error while trying to find stocks from yahoo, {searchTerm} symbols", symbols.Length);

                return new List<PriceResponse>();
            }
        }
    }
}