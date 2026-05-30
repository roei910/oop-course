namespace UsersAbstractions.Models.Shares
{
    public class Share
    {
        public required string Id { get; set; }
        public required double PurchasingPrice { get; set; }
        public required DateTime PurchaseDate { get; set; }
        public required double Amount { get; set; }
    }
}
