using Library.Models.SearchResults;
using Library.Models.Stocks;
using YahooFinance.Extensions;
using YahooFinance.Interfaces;
using YahooFinance.Models;

namespace YahooFinance
{
    public class FinanceStrategy : IFinanceStrategy
    {
        private readonly Queue<IYahooFinance> _yahooFinanceApiList;
        private readonly Queue<IYahooFinanceBulk> _yahooFinanceBulkApiList;
        private readonly Queue<IStockAnalysisApi> _analysisApiList;

        public FinanceStrategy(List<IYahooFinance> yahooFinanceApiList, List<IStockAnalysisApi> analysisApiList)
        {
            _yahooFinanceApiList = new Queue<IYahooFinance>(yahooFinanceApiList);

            var yahooFinanceBulkApiList = yahooFinanceApiList
                .Where(yahooFinanceApi => yahooFinanceApi is IYahooFinanceBulk)
                .Select(yahooFinanceApi => yahooFinanceApi as IYahooFinanceBulk)
                .ToList();

            _yahooFinanceBulkApiList = yahooFinanceBulkApiList.Count == 0 ?
                new Queue<IYahooFinanceBulk>() :
                new Queue<IYahooFinanceBulk>(yahooFinanceBulkApiList!);

            _analysisApiList = new Queue<IStockAnalysisApi>(analysisApiList);
        }

        public async Task<List<StockSearchResult>> FindStockAsync(string searchTerm)
        {
            var stocksSearchInformation = await _yahooFinanceApiList
                .ExecuteWithQueueAsync(api => api.FindStockAsync(searchTerm));

            return stocksSearchInformation;
        }

        public async Task<PriceResponse?> GetStockAsync(string symbol)
        {
            var stock = await _yahooFinanceApiList.ExecuteWithQueueAsync(api => api.GetStockAsync(symbol));

            if (stock is null || stock!.RegularMarketPrice == 0)
                throw new Exception("Couldn't get the stock price");

            return stock;
        }

        public async Task<List<StockAnalysis>> GetStocksAnalysisAsync(string[] orderedStockSymbols)
        {
            var stockAnalysisList = await _analysisApiList
                .ExecuteWithQueueAsync(api => api.GetStocksAnalysisAsync(orderedStockSymbols));

            return stockAnalysisList;
        }

        public async Task<List<PriceResponse>> GetStocksAsync(params string[] symbols)
        {
            var stocks = new List<PriceResponse>();

            if (_yahooFinanceBulkApiList.Count > 0)
            {
                stocks = await _yahooFinanceBulkApiList.ExecuteWithQueueAsync(api => api.GetStocksAsync(symbols));
                stocks = stocks.Where(stock => stock is not null && stock.RegularMarketPrice != 0).ToList();

                return stocks;
            }

            var stockTasks = symbols.Select(GetStockAsync);

            await Task.WhenAll(stockTasks);

            stocks = stockTasks
                .Where(task => task.Result is not null && task.Result.RegularMarketPrice != 0)
                .Select(task => task.Result!).ToList();

            return stocks;
        }
    }
}