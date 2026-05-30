namespace FinanceGrid.Users.Domain.Entities;

public class UserStockNote
{
    public string Id { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; }
    public DateTime LastUpdateTime { get; set; }
}
