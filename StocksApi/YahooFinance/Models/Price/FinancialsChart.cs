namespace YahooFinance.Models.Price
{
    public class FinancialsChart
    {
        public List<YearlyFinancials>? Yearly { get; set; }
        public List<QuarterlyFinancials>? Quarterly { get; set; }
    }
}