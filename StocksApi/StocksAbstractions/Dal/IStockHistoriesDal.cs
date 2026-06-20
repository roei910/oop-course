namespace StocksAbstractions.Dal
{
    public interface IStockHistoriesDal
    {
        Task UpdateStocksHistoryAsync(DateTime lastCloseDateTime);
    }
}
