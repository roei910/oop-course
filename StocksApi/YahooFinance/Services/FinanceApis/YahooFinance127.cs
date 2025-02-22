using Library.Interfaces;
using Library.Models;
using Library.Models.Stocks;
using Microsoft.Extensions.Logging;
using YahooFinance.Factories;
using YahooFinance.Generators;
using YahooFinance.Interfaces;
using YahooFinance.Models.YahooFinance127;

namespace YahooFinance.Services.FinanceApis
{
	public class YahooFinance127 : IStockAnalysisApi
    {
        private readonly IWebApi _webApi;
        private readonly ILogger<Stock> _logger;

        public YahooFinance127(WebApiFactory webApiFactory, ILogger<Stock> logger)
        {
            _webApi = webApiFactory.Generate(ConfigurationKeys.Finance127Section);
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

                var stockAnalysis = StockAnalysisGenerator.Generate(symbol, response);

                return stockAnalysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while trying to get stock analysis from yahoo, symbol {symbol}", symbol);

                return null;
            }
        }

        public async Task<List<StockAnalysis>> GetStocksAnalysisAsync(params string[] symbols)
        {
            try
            {
                var stockAnalysisTasks = symbols
                .Select(async symbol => await GetStockAnalysisAsync(symbol));

                await Task.WhenAll(stockAnalysisTasks);

                var stockAnalysisList = stockAnalysisTasks
                    .Select(stockAnalysisTask => stockAnalysisTask.Result)
                    .Where(stockAnalyis => stockAnalyis is not null)
                    .Select(stockAnalysis => stockAnalysis!)
                    .ToList();

                return stockAnalysisList;
            }
            catch (Exception)
            {
                _logger.LogError("error while trying to get analysis of stocks list from yahoo");

                return new List<StockAnalysis>();
            }
        }
    }
}