namespace StocksApi.Abstractions.Services
{
	public interface IStockNotificationSender
	{
        Task HandleStockPriceUpdatesAsync(params string[] stockSymbols);
    }
}
