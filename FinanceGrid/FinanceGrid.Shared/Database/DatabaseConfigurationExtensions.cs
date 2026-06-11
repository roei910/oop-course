using Microsoft.Extensions.Configuration;
using System.IO;

namespace FinanceGrid.Shared.Database;

public static class DatabaseConfigurationExtensions
{
    public static DatabaseConfiguration GetDatabaseConfiguration(
        this IConfiguration configuration, 
        string connectionStringName)
    {
        var provider = configuration.GetValue<string>("DatabaseProvider") ?? "SQLite";
        var basePath = Directory.Exists("/data") ? "/data" : ".";
        var connectionString = configuration.GetConnectionString(connectionStringName)
            ?? $"Data Source={basePath}/{connectionStringName}.db";
            
        return new DatabaseConfiguration
        {
            Provider = provider,
            ConnectionString = connectionString
        };
    }
}
