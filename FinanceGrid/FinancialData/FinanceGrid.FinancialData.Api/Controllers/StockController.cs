using FinanceGrid.FinancialData.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceGrid.FinancialData.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly ITrendService _trendService;
    private readonly ISearchService _searchService;

    public StockController(
        IStockService stockService,
        ITrendService trendService,
        ISearchService searchService)
    {
        _stockService = stockService;
        _trendService = trendService;
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStocks()
    {
        var stocks = await _stockService.GetAllStocksAsync();
        return Ok(stocks);
    }

    [HttpGet("symbol/{symbol}")]
    public async Task<IActionResult> GetStockBySymbol(string symbol)
    {
        var stock = await _stockService.GetStockBySymbolAsync(symbol);
        if (stock is null) return NotFound();
        return Ok(stock);
    }

    [HttpPost("symbol/bulk")]
    public async Task<IActionResult> GetStocksBySymbols([FromBody] string[] symbols)
    {
        var stocks = await _stockService.GetStocksBySymbolsAsync(symbols);
        return Ok(stocks);
    }

    [HttpGet("find/{searchTerm}")]
    public async Task<IActionResult> FindSymbolByTerm(string searchTerm)
    {
        var results = await _searchService.SearchStockByTermAsync(searchTerm);
        return Ok(results);
    }

    [HttpGet("marketTrends")]
    public async Task<IActionResult> GetMarketTrends()
    {
        var trends = await _trendService.GetMarketTrendsAsync();
        return Ok(trends);
    }

    [HttpPost("force-update-all-stocks")]
    public async Task<IActionResult> ForceUpdateAllStocks()
    {
        await _stockService.ForceUpdateAllStocksAsync();
        return Ok(new { message = "Update initiated" });
    }
}
