using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Domain.Interfaces;
using FinanceGrid.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.BackgroundServices;

public class StockAnalysisUpdater : BackgroundService
{
    private readonly IStockRepository _stockRepository;
    private readonly IStockService _stockService;
    private readonly ILogger<StockAnalysisUpdater> _logger;

    public StockAnalysisUpdater(
        IStockRepository stockRepository,
        IStockService stockService,
        ILogger<StockAnalysisUpdater> logger)
    {
        _stockRepository = stockRepository;
        _stockService = stockService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateAnalysisAsync();
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task UpdateAnalysisAsync()
    {
        try
        {
            var stocks = await _stockRepository.GetAllAsync();

            var symbols = stocks
                .OrderBy(s => s.Analysis?.UpdatedTime)
                .Take(AppVariables.MAX_ALLOWED_ANALYSIS_PER_DAY)
                .Select(s => s.Symbol)
                .ToArray();

            if (symbols.Length == 0)
                return;

            await _stockService.UpdateStocksAnalysisAsync(symbols);
            _logger.LogInformation("Updated analysis for {Count} stocks", symbols.Length);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update stock analysis");
        }
    }
}
