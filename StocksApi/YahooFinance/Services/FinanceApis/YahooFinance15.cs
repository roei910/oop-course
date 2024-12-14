using Library.Models;
using Library.Models.SearchResults;
using Microsoft.Extensions.Logging;
using YahooFinance.Factories;
using YahooFinance.Interfaces;
using YahooFinance.Models;

namespace YahooFinance.Services.FinanceApis
{
    public class YahooFinance15 : IYahooFinance, IYahooFinanceBulk
	{
        private readonly IWebApi _webApi;
        private readonly ILogger<Stock> _logger;

        public YahooFinance15(WebApiFactory webApiFactory, ILogger<Stock> logger)
        {
            _webApi = webApiFactory.Generate(ConfigurationKeys.Finance15Section);
            _logger = logger;
        }

        public async Task<List<StockSearchResult>> FindStockAsync(string searchTerm)
        {
            var endPoint = "markets/search";
            var queryParams = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("search", searchTerm)
            };

            try
            {
                var response = await _webApi
                .GetResponseAsync<BasicResponse<StockSearchResult>>(endPoint, queryParams.ToArray());

                return response?.Body ?? new List<StockSearchResult>();
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
                var endPoint = "markets/stock/quotes";
                var queryParams = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("ticker", string.Join(",", symbols))
                };

                var response = await _webApi
                    .GetResponseAsync<BasicResponse<PriceResponse>>(endPoint, queryParams.ToArray());

                return response?.Body ?? new List<PriceResponse>();
            }
            catch (Exception)
            {
                _logger.LogError("error while trying to find stocks from yahoo, {searchTerm} symbols", symbols.Length);

                return new List<PriceResponse>();
            }
        }
    }
}