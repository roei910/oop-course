using YahooFinance.Models.Price;

namespace YahooFinance.Interfaces
{
    public interface IYahooFinanceBulk
	{
        Task<List<PriceResponse>> GetStocksAsync(params string[] symbols);
    }
}