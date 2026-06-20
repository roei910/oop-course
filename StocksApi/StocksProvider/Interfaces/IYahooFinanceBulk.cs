using StocksAbstractions.Models.Price;

namespace StocksProvider.Interfaces
{
    public interface IYahooFinanceBulk
	{
        Task<List<PriceResponse>> GetStocksAsync(params string[] symbols);
    }
}