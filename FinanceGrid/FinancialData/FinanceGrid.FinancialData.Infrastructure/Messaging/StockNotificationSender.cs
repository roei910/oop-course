using FinanceGrid.FinancialData.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Messaging;

public class StockNotificationSender : IStockNotificationSender
{
    private readonly ILogger<StockNotificationSender> _logger;

    public StockNotificationSender(ILogger<StockNotificationSender> logger)
    {
        _logger = logger;
    }

    public Task HandleStockPriceUpdatesAsync(params string[] stockSymbols)
    {
        foreach (var symbol in stockSymbols)
            _logger.LogInformation("Stock price update detected for {Symbol} — would notify subscribers", symbol);

        return Task.CompletedTask;
    }
}
