namespace StocksApi.Abstractions.Dal
{
    public interface IStockHistoriesDal
    {
        Task UpdateStocksHistoryAsync(DateTime lastCloseDateTime);
    }
}
