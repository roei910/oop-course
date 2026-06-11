using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FinanceGrid.Shared.Database;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        services.Configure<DatabaseConfiguration>(options =>
        {
            configuration.GetSection("Database").Bind(options);

            if (options.IsSQLite && string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                var basePath = Directory.Exists("/data") ? "/data" : ".";
                options.ConnectionString = $"Data Source={basePath}/{serviceName}.db";
            }

            if (options.IsPostgreSQL && string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string is required for PostgreSQL provider. " +
                    $"Set Database__ConnectionString for {serviceName}.");
            }
        });

        return services;
    }
}
