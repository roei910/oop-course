using StocksApi.Interfaces;
using Library.Models;
using Library.Interfaces;

namespace StocksApi.Services
{
    public class StocksAutomaticUpdater : BackgroundService
    {
        private readonly Variables _variables;
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<Stock> _logger;
        private CancellationToken _cancellationToken;
        private readonly IStockMarketTime _stockMarketTime;
        private readonly IStockNotificationSender _stockNotificationSender;

        public StocksAutomaticUpdater(IAppConfiguration financeConfiguration,
            IStockRepository stockRepository,
            ILogger<Stock> logger,
            IStockMarketTime stockMarketTime,
            IStockNotificationSender stockNotificationSender)
        {
            _variables = financeConfiguration.Get<Variables>(ConfigurationKeys.AppVariablesSection);
            _stockRepository = stockRepository;
            _logger = logger;
            _stockMarketTime = stockMarketTime;
            _stockNotificationSender = stockNotificationSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cancellationToken = new CancellationToken();

            while (!_cancellationToken.IsCancellationRequested)
            {
                await UpdateStocksAsync();

                await Task.Delay(new TimeSpan(0, _variables.MINUTE_INTERVAL_BETWEEN_UPDATE, 0), _cancellationToken);
            }
        }

        private async Task UpdateStocksAsync()
        {
            try
            {
                var stocks = await _stockRepository.GetAllAsync();
                var symbols = stocks.Select(stock => stock.Symbol);

                var stockSymbolsForUpdate = stocks
                .Where(_stockMarketTime.ShouldStockBeUpdated)
                .Select(stock => stock.Symbol)
                .ToArray();

                await _stockRepository.UpdateStocksBySymbolAsync(stockSymbolsForUpdate);

                _logger.LogInformation("Stocks were updated");

                await _stockNotificationSender.HandleStockPriceUpdatesAsync(stockSymbolsForUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError("Couldnt update stocks\nerror: {e}", ex);
            }
        }
    }
}