using Library.Models.Shares;

namespace StocksApi.Generators
{
    public static class WatchingStockGenerator
	{
		public static WatchingStock Generate()
		{
			var watchingStock = new WatchingStock
			{
				PurchaseGuidToShares = new Dictionary<string, Share>(),
				Note = ""
			};

			return watchingStock;
		}
	}
}