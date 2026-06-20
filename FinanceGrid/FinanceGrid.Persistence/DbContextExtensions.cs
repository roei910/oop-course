using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FinanceGrid.Persistence;

public static class DbContextExtensions
{
    public static IServiceCollection AddDbContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
        where TContext : DbContext
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

        services.AddDbContextFactory<TContext>((sp, options) =>
        {
            var dbConfig = sp.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;
            switch (dbConfig.Provider.ToLower())
            {
                case "postgresql":
                    options.UseNpgsql(dbConfig.ConnectionString);
                    break;
                case "sqlite":
                default:
                    options.UseSqlite(dbConfig.ConnectionString);
                    break;
            }
        });

        return services;
    }

    public static void ApplyMigrations<TContext>(this IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var dbConfig = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;
        var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<TContext>>();
        using var context = contextFactory.CreateDbContext();
        switch (dbConfig.Provider.ToLower())
        {
            case "postgresql":
                context.Database.Migrate();
                break;
            case "sqlite":
            default:
                context.Database.EnsureCreated();
                break;
        }
    }

    public static async Task ApplyMigrationsAsync<TContext>(this IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var dbConfig = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;
        var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<TContext>>();
        await using var context = await contextFactory.CreateDbContextAsync();
        switch (dbConfig.Provider.ToLower())
        {
            case "postgresql":
                await context.Database.MigrateAsync();
                break;
            case "sqlite":
            default:
                await context.Database.EnsureCreatedAsync();
                break;
        }
    }
}
