using System.Text.Json.Serialization;
using UsersAbstractions.Models.Notifications;
using UsersAbstractions.Models.StockNotes;

namespace UsersAbstractions.Models
{
    public class User
    {
        public string? Id { get; set; }

        [JsonIgnore]
        public string? Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required List<string> WatchListNames { get; set; }
        public required List<StockNotification> StockNotifications { get; set; }
        public required Dictionary<string, List<UserStockNote>> UserStockNotesBySymbol { get; set; }
    }
}
