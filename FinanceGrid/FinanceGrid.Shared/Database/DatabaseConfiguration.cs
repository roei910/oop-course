namespace FinanceGrid.Shared.Database;

public class DatabaseConfiguration
{
    public string Provider { get; set; } = "SQLite";
    public string ConnectionString { get; set; } = string.Empty;
    
    public bool IsPostgreSQL => 
        Provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase);
    public bool IsSQLite => 
        Provider.Equals("SQLite", StringComparison.OrdinalIgnoreCase);
}
