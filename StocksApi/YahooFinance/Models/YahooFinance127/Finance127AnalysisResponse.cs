namespace YahooFinance.Models.YahooFinance127
{
	public class Finance127AnalysisResponse
	{
		public required RawFmtValue CurrentPrice { get; set; }
		public required RawFmtValue TargetHighPrice { get; set; }
		public required RawFmtValue TargetLowPrice { get; set; }
		public required RawFmtValue TargetMeanPrice { get; set; }
		public required RawFmtValue TargetMedianPrice { get; set; }
    }
}