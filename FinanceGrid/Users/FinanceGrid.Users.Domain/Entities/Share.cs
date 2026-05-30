namespace FinanceGrid.Users.Domain.Entities;

public class Share
{
    public string Id { get; set; } = string.Empty;
    public double PurchasingPrice { get; set; }
    public DateTime PurchaseDate { get; set; }
    public double Amount { get; set; }
}
