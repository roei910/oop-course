using StocksApi.Interfaces;
using Library.Models;
using YahooFinance.Interfaces;
using StocksApi.Generators;
using Library.Models.Users.Notifications;

namespace StocksApi.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly IStocksDal _stocksDal;
        private readonly ILogger<Stock> _logger;
        private readonly IFinanceStrategy _yahooFinanceStrategy;
        private readonly IStockMarketTime _stockMarketTime;

        public StockRepository(
            IStocksDal stocksDal,
            ILogger<Stock> logger,
            IFinanceStrategy yahooFinanceStrategy,
            IStockMarketTime stockMarketTime)
        {
            _stocksDal = stocksDal;
            _logger = logger;
            _yahooFinanceStrategy = yahooFinanceStrategy;
            _stockMarketTime = stockMarketTime;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            var stocks = await _stocksDal.FindAllAsync();

            return stocks;
        }

        public async Task<Stock?> GetStockBySymbolAsync(string symbol)
        {
            symbol = symbol.ToUpper();

            var found = await _stocksDal.FindBySymbolAsync(symbol);

            if (found is null)
            {
                var stock = await CreateStockAsync(symbol);

                if (stock is null)
                    return null;

                await _stocksDal.CreateAsync(stock);

                return stock;
            }

            return found;
        }

        public async Task<List<Stock>> GetStocksBySymbolAsync(params string[] symbols)
        {
            symbols = symbols.Select(symbol => symbol.ToUpper()).ToArray();

            var foundStocks = await _stocksDal.FindManyBySymbolAsync(symbols);

            var foundStocksBySymbol = foundStocks
                .Where(stock => symbols.Contains(stock.Symbol))
                .Select(stock => stock.Symbol);

            var stocksToBeCreatedBySymbol = symbols.Except(foundStocksBySymbol).ToList();

            if (!stocksToBeCreatedBySymbol.Any())
                return foundStocks;

            var createdStocks = await CreateStocksAsync(stocksToBeCreatedBySymbol.ToArray());

            await _stocksDal.CreateAsync(createdStocks);

            return createdStocks.Union(foundStocks).ToList();
        }

        public async Task UpdateStocksBySymbolAsync(params string[] stockSymbols)
        {
            var updatedStockPriceResponses = await _yahooFinanceStrategy.GetStocksAsync(stockSymbols);

            if(updatedStockPriceResponses.Count != 0)
                await _stocksDal.UpdateStockPriceBulkAsync(updatedStockPriceResponses);

            await UpdateStocksHistoryAsync();
        }

        private async Task UpdateStocksHistoryAsync()
        {
            var lastMarketOpenDateTime = _stockMarketTime.LastMarketCloseDateTime();

            await _stocksDal.UpdateStocksHistoryAsync(lastMarketOpenDateTime);
        }

        public async Task UpdateStocksAnalysisAsync(params string[] orderedStockSymbols)
        {
            var stocksAnalysisList = await _yahooFinanceStrategy.GetStocksAnalysisAsync(orderedStockSymbols);

            await _stocksDal.UpdateAnalysisBulkAsync(stocksAnalysisList);
        }

        private async Task<Stock?> CreateStockAsync(string stockSymbol)
        {
            try
            {
                var yahooStock = await _yahooFinanceStrategy.GetStockAsync(stockSymbol);

                var stock = StockGenerator.Generate(yahooStock);

                if (stock is null)
                    _logger.LogWarning("yahoo stock was null, couldnt create a new stock");

                return stock;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "couldnt create new stock");

                return null;
            }
        }

        private async Task<List<Stock>> CreateStocksAsync(params string[] stockSymbols)
        {
            try
            {
                var yahooStockList = await _yahooFinanceStrategy.GetStocksAsync(stockSymbols);

                var stocks = yahooStockList
                    .Where(yahooStock => yahooStock is not null)
                    .Select(yahooStock => StockGenerator.Generate(yahooStock)!)
                    .ToList();

                if (stocks?.Count == 0)
                    _logger.LogWarning("received empty list of yahoo stocks");

                return stocks ?? new List<Stock>();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "couldnt create stocks");

                return new List<Stock>();
            }
        }

        public async Task RemoveNotificationAsync(string symbol, string notificationId)
        {
            await _stocksDal.RemoveNotificationAsync(symbol, notificationId);
        }

        public async Task AddNotificationAsync(StockNotification stockNotification)
        {
            await _stocksDal.AddNotificationAsync(stockNotification);
        }
    }
}