using StocksAbstractions.Services;

namespace StocksService.Services
{
    public class StubStockNotificationSender : IStockNotificationSender
    {
        private readonly ILogger<StubStockNotificationSender> _logger;

        public StubStockNotificationSender(ILogger<StubStockNotificationSender> logger)
        {
            _logger = logger;
        }

        public Task HandleStockPriceUpdatesAsync(params string[] stockSymbols)
        {
            _logger.LogInformation("Stock price updates for {Count} symbols — notifications will be sent via RabbitMQ in future", stockSymbols.Length);
            return Task.CompletedTask;
        }
    }
}
