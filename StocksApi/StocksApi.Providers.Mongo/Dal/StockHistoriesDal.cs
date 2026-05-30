using Microsoft.Extensions.Logging;
using StocksApi.Providers.Mongo;
using StocksAbstractions.Models;
using StocksAbstractions.Models.Stocks;
using MongoDB.Driver;
using StocksAbstractions.Dal;

namespace StocksApi.Providers.Mongo.Dal
{
    public class StockHistoriesDal : IStockHistoriesDal
    {
        private readonly IMongoCollection<StockHistoryEntry> _historyCollection;
        private readonly IMongoCollection<Stock> _stocksCollection;
        private readonly ILogger<StockHistoryEntry> _logger;

        public StockHistoriesDal(IMongoDbContext mongoDbContext, ILogger<StockHistoryEntry> logger)
        {
            _historyCollection = mongoDbContext.GetCollection<StockHistoryEntry>("StockHistory");
            _stocksCollection = mongoDbContext.GetCollection<Stock>("Stocks");
            _logger = logger;
        }

        public async Task UpdateStocksHistoryAsync(DateTime lastCloseDateTime)
        {
            var lastHistoryUpdateDate = DateOnly.FromDateTime(lastCloseDateTime);

            var filterHistoryDateTime = Builders<Stock>.Filter.Lt(
                stock => stock.LastHistoryUpdateDate, lastHistoryUpdateDate);
            var filterHistoryDateTimeNull = Builders<Stock>.Filter.Eq(
                stock => stock.LastHistoryUpdateDate, null);
            var filterHistory = Builders<Stock>.Filter.Or(
                filterHistoryDateTime, filterHistoryDateTimeNull);

            var findFluent = _stocksCollection.Find(filterHistory);
            var projection = Builders<Stock>.Projection
                .Include(stock => stock.Symbol)
                .Include(stock => stock.Price)
                .Include(stock => stock.RegularMarketOpen)
                .Include(stock => stock.RegularMarketDayLow)
                .Include(stock => stock.RegularMarketDayHigh)
                .Include(stock => stock.RegularMarketVolume)
                .Include(stock => stock.RegularMarketDayRange);

            var stocksProjection = await findFluent
                .Project(projection)
                .ToListAsync();

            if (stocksProjection.Count == 0)
                return;

            var historyEntries = new List<StockHistoryEntry>();
            var stockUpdateModels = new List<WriteModel<Stock>>();

            foreach (var stockDoc in stocksProjection)
            {
                var symbol = stockDoc["Symbol"].AsString;

                historyEntries.Add(new StockHistoryEntry
                {
                    StockSymbol = symbol,
                    Date = lastHistoryUpdateDate,
                    DayLow = stockDoc["RegularMarketDayLow"].AsDouble,
                    DayHigh = stockDoc["RegularMarketDayHigh"].AsDouble,
                    DayRange = stockDoc["RegularMarketDayRange"].AsString,
                    DayVolume = stockDoc["RegularMarketVolume"].AsInt64,
                    PriceClose = stockDoc["Price"].AsDouble,
                    PriceOpen = stockDoc["RegularMarketOpen"].AsDouble
                });

                var filterBySymbol = Builders<Stock>.Filter.Eq(s => s.Symbol, symbol);
                var update = Builders<Stock>.Update
                    .Set(s => s.LastHistoryUpdateDate, lastHistoryUpdateDate);
                stockUpdateModels.Add(new UpdateOneModel<Stock>(filterBySymbol, update));
            }

            try
            {
                await _historyCollection.InsertManyAsync(historyEntries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed to insert stock history, {count} entries", historyEntries.Count);
                return;
            }

            try
            {
                await _stocksCollection.BulkWriteAsync(stockUpdateModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed to update stock last history date, {count} updates", stockUpdateModels.Count);
            }
        }
    }
}
