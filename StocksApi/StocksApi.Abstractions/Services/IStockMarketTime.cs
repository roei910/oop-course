using StocksApi.Abstractions.Models;

namespace StocksApi.Abstractions.Services
{
	public interface IStockMarketTime
	{
        bool IsMarketOpen(DateTime date);
        bool ShouldStockBeUpdated(Stock stock);
        DateTime LastMarketCloseDateTime();
    }
}
