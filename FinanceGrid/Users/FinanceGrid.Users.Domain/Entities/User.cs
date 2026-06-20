using System.Text.Json.Serialization;

namespace FinanceGrid.Users.Domain.Entities;

public class User
{
    public string? Id { get; set; }

    [JsonIgnore]
    public string? Password { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> WatchListNames { get; set; } = [];
    public List<StockNotification> StockNotifications { get; set; } = [];
    public Dictionary<string, List<UserStockNote>> UserStockNotesBySymbol { get; set; } = [];
}
