namespace Library.Models.SearchResults
{
    public class StockSearchResult
    {
        public required string Symbol { get; set; }
        public required string Name { get; set; }
        public required string ExchDisp { get; set; }
        public required string TypeDisp { get; set; }
    }
}