using Microsoft.Extensions.Configuration;

namespace FinanceGrid.Shared.Database;

public static class DatabaseConfigurationExtensions
{
    public static DatabaseConfiguration GetDatabaseConfiguration(
        this IConfiguration configuration, 
        string connectionStringName)
    {
        var provider = configuration.GetValue<string>("DatabaseProvider") ?? "SQLite";
        var connectionString = configuration.GetConnectionString(connectionStringName)
            ?? $"Data Source={connectionStringName}.db";
            
        return new DatabaseConfiguration
        {
            Provider = provider,
            ConnectionString = connectionString
        };
    }
}
