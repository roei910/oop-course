namespace FinanceGrid.FinancialData.Domain.Interfaces;

public interface IStockNotificationSender
{
    Task HandleStockPriceUpdatesAsync(params string[] stockSymbols);
}
