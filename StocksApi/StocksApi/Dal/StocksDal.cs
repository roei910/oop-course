﻿using MongoDB.Driver;
using StocksApi.Interfaces;
using Library.Models;
using Library.Interfaces;
using Library.Models.Stocks;
using Library.Models.Users.Notifications;
using YahooFinance.Models.Price;
using MongoDB.Bson;
using System;

namespace StocksApi.Dal
{
    public class StocksDal : IStocksDal
    {
        private readonly IMongoCollection<Stock> _collection;
        private readonly ILogger<Stock> _logger;

        public StocksDal(
            IAppConfiguration appConfiguration,
            ILogger<Stock> logger
            )
        {
            _logger = logger;

            var databaseSettings = appConfiguration
                .Get<DatabaseSettings>(ConfigurationKeys.DatabaseSettingsSection);

            var connectionString = appConfiguration.Get<string>(ConfigurationKeys.ConnectionStringSection);

            var mongoClient = new MongoClient(connectionString);

             var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.DatabaseName);

            _collection = mongoDatabase.GetCollection<Stock>(
                databaseSettings.StocksCollectionName);
        }

        public async Task<Stock?> FindByIdAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Stock newStock) =>
            await _collection.InsertOneAsync(newStock);

        public async Task CreateAsync(List<Stock> stocks) =>
            await _collection.InsertManyAsync(stocks);

        public async Task UpdateAsync(string id, Stock updateStock) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updateStock);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Stock>> FindAllAsync()
        {
            var filter = Builders<Stock>.Filter.Empty;

            var stocksCursor = await _collection.FindAsync(filter);
            var stocks = await stocksCursor.ToListAsync();

            return stocks;
        }

        public async Task UpdateAnalysisBulkAsync(List<StockAnalysis> stocksAnalysis)
        {
            var writeModels = stocksAnalysis
                .Where(stockAnalysis => stockAnalysis.Symbol is not null)
                .Select(stockAnalysis =>
                {
                    var symbol = stockAnalysis.Symbol;
                    var filter = Builders<Stock>.Filter.Eq(stock => stock.Symbol, symbol);
                    var update = Builders<Stock>.Update
                        .Set(stock => stock.Analysis, stockAnalysis)
                        .Set(stock => stock.UpdatedTime, DateTime.UtcNow);

                    return new UpdateOneModel<Stock>(filter, update);
                });

            if (writeModels is null || !writeModels.Any())
                return;

            try
            {
                await _collection.BulkWriteAsync(writeModels);

            }
            catch (Exception e)
            {
                _logger.LogError("bulk write exception, {writeModels.Count}\n error: {e}", writeModels, e);
            }
        }

        public async Task UpdateStockPriceBulkAsync(List<PriceResponse> stockPriceResponses)
        {
            var writeModels = stockPriceResponses.Select(updatedStock =>
            {
                var filter = Builders<Stock>.Filter.Eq(stock => stock.Symbol, updatedStock.Symbol);
                var update = Builders<Stock>.Update
                .Set(stock => stock.Price, updatedStock.RegularMarketPrice)
                .Set(stock => stock.RegularMarketChange, updatedStock.RegularMarketChange)
                .Set(stock => stock.RegularMarketChangePercent, updatedStock.RegularMarketChangePercent)
                .Set(stock => stock.RegularMarketDayHigh, updatedStock.RegularMarketDayHigh)
                .Set(stock => stock.RegularMarketDayLow, updatedStock.RegularMarketDayLow)
                .Set(stock => stock.RegularMarketDayRange, updatedStock.RegularMarketDayRange)
                .Set(stock => stock.RegularMarketOpen, updatedStock.RegularMarketOpen)
                .Set(stock => stock.RegularMarketPreviousClose, updatedStock.RegularMarketPreviousClose)
                .Set(stock => stock.RegularMarketVolume, updatedStock.RegularMarketVolume)
                .Set(stock => stock.FiftyDayAverage, updatedStock.FiftyDayAverage)
                .Set(stock => stock.FiftyTwoWeekHigh, updatedStock.FiftyTwoWeekHigh)
                .Set(stock => stock.FiftyTwoWeekLow, updatedStock.FiftyTwoWeekLow)
                .Set(stock => stock.FiftyTwoWeekRange, updatedStock.FiftyTwoWeekRange)
                .Set(stock => stock.TwoHundredDayAverage, updatedStock.TwoHundredDayAverage)
                .Set(stock => stock.TargetPriceHigh, updatedStock.TargetPriceHigh)
                .Set(stock => stock.TargetPriceLow, updatedStock.TargetPriceLow)
                .Set(stock => stock.TargetPriceMean, updatedStock.TargetPriceMean)
                .Set(stock => stock.TargetPriceMedian, updatedStock.TargetPriceMedian)
                .Set(stock => stock.ForwardPE, updatedStock.ForwardPE)
                .Set(stock => stock.EpsCurrentYear, updatedStock.EpsCurrentYear)
                .Set(stock => stock.EpsForward, updatedStock.EpsForward)
                .Set(stock => stock.UpdatedTime, DateTime.UtcNow);

                if (updatedStock is not null)
                    update.Set(stock => stock.AnalystRating, updatedStock.AverageAnalystRating);

                var writeModel = new UpdateOneModel<Stock>(filter, update);

                return writeModel as WriteModel<Stock>;
            });

            if (writeModels is null || !writeModels.Any())
                return;

            try
            {
                await _collection.BulkWriteAsync(writeModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "bulk write exception, {writeModels.Count}", writeModels);
            }
        }

        public async Task<Stock?> FindBySymbolAsync(string symbol)
        {
            var filter = Builders<Stock>.Filter.Eq(stock => stock.Symbol, symbol);
            var foundStock = await (await _collection.FindAsync(filter)).FirstOrDefaultAsync();

            return foundStock;
        }

        public async Task<List<Stock>> FindManyBySymbolAsync(string[] symbols)
        {
            var filter = Builders<Stock>.Filter.In(stock => stock.Symbol, symbols);
            var foundStocks = await (await _collection.FindAsync(filter)).ToListAsync();

            return foundStocks;
        }

        public async Task AddNotificationAsync(StockNotification stockNotification)
        {
            var filter = Builders<Stock>.Filter.Eq(stock => stock.Symbol, stockNotification.StockSymbol);
            var update = Builders<Stock>.Update.AddToSet(stock => stock.StockNotifications, stockNotification);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task RemoveNotificationAsync(string symbol, string notificationId)
        {
            var filter = Builders<Stock>.Filter.Eq(stock => stock.Symbol, symbol);
            var update = Builders<Stock>.Update.PullFilter(stock => stock.StockNotifications,
                notification => notification.Id == notificationId);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateStocksHistoryAsync(DateTime lastCloseDateTime)
        {
            var lastHistoryUpdateDate = DateOnly.FromDateTime(lastCloseDateTime);

            var filterHistoryDateTime = Builders<Stock>.Filter.Lt(stock => stock.LastHistoryUpdateDate, lastHistoryUpdateDate);
            var filterHistoryDateTimeNull = Builders<Stock>.Filter.Eq(stock => stock.LastHistoryUpdateDate, null);
            var filterHistory = Builders<Stock>.Filter.Or(filterHistoryDateTime, filterHistoryDateTimeNull);

            var findFluent = _collection.Find(filterHistory);
            var projection = Builders<Stock>.Projection
                .Include(stock => stock.Id)
                .Include(stock => stock.Price)
                .Include(stock => stock.RegularMarketOpen)
                .Include(stock => stock.RegularMarketDayLow)
                .Include(stock => stock.RegularMarketDayHigh)
                .Include(stock => stock.RegularMarketVolume)
                .Include(stock => stock.RegularMarketDayRange);

            var stocksProjection = await findFluent
                .Project(projection)
                .ToListAsync();

            var writeModels = stocksProjection.Select(stock =>
            {
                var filterById = Builders<Stock>.Filter.Eq("_id", stock["_id"].AsObjectId);
                var combinedFilter = Builders<Stock>.Filter.And(filterById, filterHistory);

                var stockHistory = new StockHistory
                {
                    Date = lastHistoryUpdateDate,
                    DayLow = stock["RegularMarketDayLow"].AsDouble,
                    DayHigh = stock["RegularMarketDayHigh"].AsDouble,
                    DayRange = stock["RegularMarketDayRange"].AsString,
                    DayVolume = stock["RegularMarketVolume"].AsInt64,
                    PriceClose = stock["Price"].AsDouble,
                    PriceOpen = stock["RegularMarketOpen"].AsDouble
                };

                var update = Builders<Stock>.Update
                    .Set(stock => stock.LastHistoryUpdateDate, lastHistoryUpdateDate)
                    .AddToSet(stock => stock.History, stockHistory);

                var updateModel = new UpdateOneModel<Stock>(combinedFilter, update);

                return updateModel;
            });

            try
            {
                await _collection.BulkWriteAsync(writeModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "bulk write exception, {writeModels.Count}", writeModels);
            }
        }
    }
}