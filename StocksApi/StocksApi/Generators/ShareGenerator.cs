using MongoDB.Bson;
using Library.Models.Shares;

namespace StocksApi.Generators
{
    public static class ShareGenerator
	{
		public static Share Generate(SharePurchase sharePurchase)
		{
			var share = new Share
			{
				Id = ObjectId.GenerateNewId().ToString(),
				PurchasingPrice = sharePurchase.PurchasingPrice,
				PurchaseDate = sharePurchase.PurchaseDate,
				Amount = sharePurchase.Amount
			};

			return share;
		}
	}
}