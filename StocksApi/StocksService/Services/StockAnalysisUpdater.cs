using SharedLibrary.Models;
using SharedLibrary.Services;
using StocksAbstractions.Repositories;
using StocksAbstractions.Models;

namespace StocksService.Services
{
    public class StockAnalysisUpdater : BackgroundService
    {
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<Stock> _logger;
        private readonly Variables _variables;

        public StockAnalysisUpdater(IStockRepository stockRepository,
            ILogger<Stock> logger,
            IAppConfiguration appConfiguration)
        {
            _stockRepository = stockRepository;
            _logger = logger;
            _variables = appConfiguration.Get<Variables>(ConfigurationKeys.AppVariablesSection);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateStocksAnalysisAsync();
                await Task.Delay(new TimeSpan(24, 0, 0), stoppingToken);
            }
        }

        private async Task UpdateStocksAnalysisAsync()
        {
            try
            {
                var stocks = await _stockRepository.GetAllAsync();

                var orderedStockSymbols = stocks
                    .OrderBy(stock => stock.Analysis?.UpdatedTime)
                    .Take(_variables.MAX_ALLOWED_ANALYSIS_PER_DAY)
                    .Select(stock => stock.Symbol)
                    .ToArray();

                await _stockRepository.UpdateStocksAnalysisAsync(orderedStockSymbols);
            }
            catch (Exception e)
            {
                _logger.LogError("couldnt update stocks analysis\nerror: {e}", e);
            }
        }
    }
}
