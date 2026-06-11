namespace FinanceGrid.FinancialData.Domain.Entities;

public class SearchResult
{
    public string Id { get; set; } = string.Empty;
    public string SearchTerm { get; set; } = string.Empty;
    public List<StockSearchResult> StockSearchResults { get; set; } = [];
}
