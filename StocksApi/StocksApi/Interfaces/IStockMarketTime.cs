using Library.Models;

namespace StocksApi.Interfaces
{
	public interface IStockMarketTime
	{
        bool IsMarketOpen(DateTime date);
        bool ShouldStockBeUpdated(Stock stock);
    }
}