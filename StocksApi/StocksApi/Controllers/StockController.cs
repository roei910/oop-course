using Microsoft.AspNetCore.Mvc;
using StocksApi.Interfaces;
using Library.Models;
using Library.Models.MarketTrends;
using Library.Models.SearchResults;

namespace StocksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepository;
        private readonly ITrendRepository _trendRepository;
        private readonly ISearchResultRepository _searchResultRepository;

        public StockController(IStockRepository stockRepository,
            ITrendRepository trendRepository,
            ISearchResultRepository searchResultRepository)
        {
            _stockRepository = stockRepository;
            _trendRepository = trendRepository;
            _searchResultRepository = searchResultRepository;
        }

        [HttpGet]
        public async Task<List<Stock>> GetAllStocksAsync()
        {
            var stocks = await _stockRepository.GetAllAsync();

            return stocks;
        }

        [HttpGet("symbol/{symbol}")]
        public async Task<ActionResult<Stock>> GetStockBySymbolAsync(string symbol)
        {
            var found = await _stockRepository.GetStockBySymbolAsync(symbol);

            if (found is null)
                return NotFound("couldnt get the stock you were searching");

            return Ok(found);
        }

        [HttpPost("symbol/bulk")]
        public async Task<IActionResult> GetStocksBySymbolsAsync(string[] symbols)
        {
            var stocks = await _stockRepository.GetStocksBySymbolAsync(symbols);

            return Ok(stocks);
        }

        [HttpGet("find/{searchTerm}")]
        public async Task<ActionResult<List<StockSearchResult>>> FindSymbolByTermAsync(string searchTerm)
        {
            var found = await _searchResultRepository.SearchStockByTermAsync(searchTerm);

            if (found is null)
                return NotFound();

            return Ok(found);
        }

        [HttpGet("marketTrends")]
        public async Task<ActionResult<List<MarketTrend>>> GetMarketTrendsAsync()
        {
            var marketTrends = await _trendRepository.GetMarketTrendsAsync();

            return Ok(marketTrends);
        }

        [HttpPost("force-update-all-stocks")]
        public async Task<IActionResult> ForceUpdateAllStocksAsync()
        {
            var stocks = await _stockRepository.GetAllAsync();
            var symbols = stocks
                .Select(stock => stock.Symbol)
                .ToArray();

            await _stockRepository.UpdateStocksBySymbolAsync(symbols);

            return Ok();
        }
    }
}