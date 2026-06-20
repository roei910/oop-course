using StocksAbstractions.Models;
using StocksAbstractions.Repositories;
using UsersAbstractions.Models.Notifications;

namespace UsersService.Stubs
{
    public class StubStockRepository : IStockRepository
    {
        public Task AddNotificationAsync(StockNotification stockNotification) =>
            throw new NotSupportedException("Notifications should be sent via Gateway");

        public Task<List<Stock>> GetAllAsync() =>
            throw new NotSupportedException("Stock data not available in UsersService");

        public Task<Stock?> GetStockBySymbolAsync(string symbol) =>
            throw new NotSupportedException("Stock data not available in UsersService");

        public Task<List<Stock>> GetStocksBySymbolAsync(string[] symbols) =>
            throw new NotSupportedException("Stock data not available in UsersService");

        public Task RemoveNotificationAsync(string stockSymbol, string notificationId) =>
            throw new NotSupportedException("Notifications should be managed via Gateway");

        public Task UpdateStocksAnalysisAsync(string[] stockSymbols) =>
            throw new NotSupportedException("Stock data not available in UsersService");

        public Task UpdateStocksBySymbolAsync(string[] stockSymbols) =>
            throw new NotSupportedException("Stock data not available in UsersService");
    }
}
