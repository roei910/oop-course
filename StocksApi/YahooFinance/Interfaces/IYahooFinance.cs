﻿using Library.Models.SearchResults;
using YahooFinance.Models;

namespace YahooFinance.Interfaces
{
    public interface IYahooFinance
    {
        Task<PriceResponse?> GetStockAsync(string symbol);
        Task<List<StockSearchResult>> FindStockAsync(string searchTerm);
    }
}