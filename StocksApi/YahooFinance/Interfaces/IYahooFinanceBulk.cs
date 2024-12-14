using YahooFinance.Models;

namespace YahooFinance.Interfaces
{
	public interface IYahooFinanceBulk
	{
        Task<List<PriceResponse>> GetStocksAsync(params string[] symbols);
    }
}