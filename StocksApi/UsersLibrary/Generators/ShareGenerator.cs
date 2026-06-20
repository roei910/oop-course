using UsersAbstractions.Models.Shares;

namespace UsersLibrary.Generators
{
    public static class ShareGenerator
	{
		public static Share Generate(SharePurchase sharePurchase)
		{
			var share = new Share
			{
				Id = Guid.NewGuid().ToString(),
				PurchasingPrice = sharePurchase.PurchasingPrice,
				PurchaseDate = sharePurchase.PurchaseDate,
				Amount = sharePurchase.Amount
			};

			return share;
		}
	}
}
