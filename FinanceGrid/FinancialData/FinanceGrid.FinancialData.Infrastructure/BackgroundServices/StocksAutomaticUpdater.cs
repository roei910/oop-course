using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Domain.Interfaces;
using FinanceGrid.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.BackgroundServices;

public class StocksAutomaticUpdater : BackgroundService
{
    private readonly IStockRepository _stockRepository;
    private readonly IStockService _stockService;
    private readonly IStockMarketTime _stockMarketTime;
    private readonly IStockNotificationSender _notificationSender;
    private readonly ILogger<StocksAutomaticUpdater> _logger;

    public StocksAutomaticUpdater(
        IStockRepository stockRepository,
        IStockService stockService,
        IStockMarketTime stockMarketTime,
        IStockNotificationSender notificationSender,
        ILogger<StocksAutomaticUpdater> logger)
    {
        _stockRepository = stockRepository;
        _stockService = stockService;
        _stockMarketTime = stockMarketTime;
        _notificationSender = notificationSender;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateStocksAsync();
            await Task.Delay(
                TimeSpan.FromMinutes(AppVariables.MINUTE_INTERVAL_BETWEEN_UPDATE),
                stoppingToken);
        }
    }

    private async Task UpdateStocksAsync()
    {
        try
        {
            var stocks = await _stockRepository.GetAllAsync();

            var symbolsToUpdate = stocks
                .Where(_stockMarketTime.ShouldStockBeUpdated)
                .Select(s => s.Symbol)
                .ToArray();

            if (symbolsToUpdate.Length == 0)
                return;

            await _stockService.UpdateStocksBySymbolAsync(symbolsToUpdate);
            await _notificationSender.HandleStockPriceUpdatesAsync(symbolsToUpdate);

            _logger.LogInformation("Updated {Count} stocks", symbolsToUpdate.Length);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update stocks");
        }
    }
}
