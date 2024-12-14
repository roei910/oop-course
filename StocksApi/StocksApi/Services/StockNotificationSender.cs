using StocksApi.Interfaces;

namespace StocksApi.Services
{
	public class StockNotificationSender : IStockNotificationSender
	{
        private readonly IStockRepository _stockRepository;
        private readonly IUserRepository _userRepository;

        public StockNotificationSender(IStockRepository stockRepository,
			IUserRepository userRepository)
		{
            _stockRepository = stockRepository;
			_userRepository = userRepository;
		}

		public async Task HandleStockPriceUpdatesAsync(params string[] stockSymbols)
		{
			var stocks = await _stockRepository.GetStocksBySymbolAsync(stockSymbols);

			var notifications = stocks.SelectMany(stock =>
			{
				var stockPrice = stock.Price;

				var notificationsToSend = stock.StockNotifications
					.Where(notification => notification.IsTargetBiggerThanOrEqual &&
					stockPrice >= notification.TargetPrice)
					.ToList();

				return notificationsToSend;
			}).ToList();

			var tasks = notifications.Select(async notification =>
			{
				await _userRepository.ShowNotificationAsync(notification);
				await _stockRepository.RemoveNotificationAsync(notification.StockSymbol, notification.Id!);
            });

			await Task.WhenAll(tasks);
		}
	}
}