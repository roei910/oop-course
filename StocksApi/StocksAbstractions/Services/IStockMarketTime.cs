using StocksAbstractions.Models;

namespace StocksAbstractions.Services
{
	public interface IStockMarketTime
	{
        bool IsMarketOpen(DateTime date);
        bool ShouldStockBeUpdated(Stock stock);
        DateTime LastMarketCloseDateTime();
    }
}
