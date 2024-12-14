namespace StocksApi.Interfaces
{
	public interface IStockNotificationSender
	{
        Task HandleStockPriceUpdatesAsync(params string[] stockSymbols);
    }
}