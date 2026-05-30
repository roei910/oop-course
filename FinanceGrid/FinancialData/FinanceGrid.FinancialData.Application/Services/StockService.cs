using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Application.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;
    private readonly IFinanceStrategy _financeStrategy;
    private readonly ILogger<StockService> _logger;

    public StockService(
        IStockRepository stockRepository,
        IFinanceStrategy financeStrategy,
        ILogger<StockService> logger)
    {
        _stockRepository = stockRepository;
        _financeStrategy = financeStrategy;
        _logger = logger;
    }

    public async Task<List<Stock>> GetAllStocksAsync() => await _stockRepository.GetAllAsync();

    public async Task<Stock?> GetStockBySymbolAsync(string symbol)
    {
        var existing = await _stockRepository.GetStockBySymbolAsync(symbol.ToUpper());
        if (existing is not null) return existing;

        var stock = await CreateStockAsync(symbol.ToUpper());
        if (stock is null) return null;

        var stocks = new List<Stock> { stock };
        await _stockRepository.GetStocksBySymbolAsync([symbol.ToUpper()]);
        return stock;
    }

    public async Task<List<Stock>> GetStocksBySymbolsAsync(string[] symbols)
    {
        symbols = symbols.Select(s => s.ToUpper()).ToArray();
        var foundStocks = await _stockRepository.GetStocksBySymbolAsync(symbols);
        var foundSymbols = foundStocks.Select(s => s.Symbol).ToHashSet();
        var missing = symbols.Where(s => !foundSymbols.Contains(s)).ToArray();

        if (missing.Length == 0) return foundStocks;

        var created = await CreateStocksAsync(missing);
        foundStocks.AddRange(created);
        return foundStocks;
    }

    public async Task UpdateStocksBySymbolAsync(string[] stockSymbols)
    {
        await _stockRepository.UpdateStocksBySymbolAsync(stockSymbols);
    }

    public async Task UpdateStocksAnalysisAsync(string[] orderedStockSymbols)
    {
        await _stockRepository.UpdateStocksAnalysisAsync(orderedStockSymbols);
    }

    public async Task ForceUpdateAllStocksAsync()
    {
        var allStocks = await _stockRepository.GetAllAsync();
        var symbols = allStocks.Select(s => s.Symbol).ToArray();
        await UpdateStocksBySymbolAsync(symbols);
    }

    private async Task<Stock?> CreateStockAsync(string stockSymbol)
    {
        try
        {
            var stock = await _financeStrategy.GetStockAsync(stockSymbol);
            return stock;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create stock {Symbol}", stockSymbol);
            return null;
        }
    }

    private async Task<List<Stock>> CreateStocksAsync(string[] stockSymbols)
    {
        try
        {
            var symbols = string.Join(",", stockSymbols);
            var stocks = await _financeStrategy.GetStocksAsync(symbols);
            return stocks ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create stocks");
            return [];
        }
    }
}
