using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Domain.Entities;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;
using Microsoft.Extensions.Logging;

namespace FinanceGrid.FinancialData.Infrastructure.Providers;

public class FinanceStrategy : IFinanceStrategy
{
    private readonly Queue<IYahooFinance> _yahooFinanceApiList;
    private readonly Queue<IYahooFinanceBulk> _yahooFinanceBulkApiList;
    private readonly Queue<IStockAnalysisApi> _analysisApiList;
    private readonly ILogger<FinanceStrategy> _logger;

    public FinanceStrategy(
        IEnumerable<IYahooFinance> yahooFinanceApis,
        IEnumerable<IStockAnalysisApi> analysisApis,
        ILogger<FinanceStrategy> logger)
    {
        _yahooFinanceApiList = new Queue<IYahooFinance>(yahooFinanceApis);
        _yahooFinanceBulkApiList = new Queue<IYahooFinanceBulk>(
            yahooFinanceApis.OfType<IYahooFinanceBulk>());
        _analysisApiList = new Queue<IStockAnalysisApi>(analysisApis);
        _logger = logger;
    }

    public async Task<Stock?> GetStockAsync(string symbol)
    {
        var first = _yahooFinanceApiList.Dequeue();
        try
        {
            return await first.GetStockAsync(symbol);
        }
        finally
        {
            _yahooFinanceApiList.Enqueue(first);
        }
    }

    public async Task<List<Stock>?> GetStocksAsync(string symbols)
    {
        var symbolArray = symbols.Split(',', StringSplitOptions.RemoveEmptyEntries);
        if (symbolArray.Length == 0) return [];

        if (_yahooFinanceBulkApiList.Count > 0)
        {
            var first = _yahooFinanceBulkApiList.Dequeue();
            try
            {
                return await first.GetStocksAsync(symbolArray);
            }
            finally
            {
                _yahooFinanceBulkApiList.Enqueue(first);
            }
        }

        var tasks = symbolArray.Select(async s =>
        {
            var stock = await GetStockAsync(s);
            return stock;
        });

        var results = await Task.WhenAll(tasks);
        return results.Where(s => s is not null).Cast<Stock>().ToList();
    }

    public async Task<List<StockSearchResult>> FindStockAsync(string searchTerm)
    {
        var first = _yahooFinanceApiList.Dequeue();
        try
        {
            return await first.FindStockAsync(searchTerm);
        }
        finally
        {
            _yahooFinanceApiList.Enqueue(first);
        }
    }

    public async Task<StockAnalysis?> GetStockAnalysisAsync(string symbol)
    {
        var first = _analysisApiList.Dequeue();
        try
        {
            return await first.GetStockAnalysisAsync(symbol);
        }
        finally
        {
            _analysisApiList.Enqueue(first);
        }
    }

    public async Task<List<StockAnalysis>> GetStocksAnalysisAsync(string[] symbols)
    {
        var first = _analysisApiList.Dequeue();
        try
        {
            return await first.GetStocksAnalysisAsync(symbols);
        }
        finally
        {
            _analysisApiList.Enqueue(first);
        }
    }
}
