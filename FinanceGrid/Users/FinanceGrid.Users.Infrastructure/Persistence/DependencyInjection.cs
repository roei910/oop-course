using FinanceGrid.Shared.Database;
using FinanceGrid.Users.Domain.Interfaces;
using FinanceGrid.Users.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FinanceGrid.Users.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration, "Users");

        services.AddDbContextFactory<UsersDbContext>((sp, options) =>
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

        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserStockWatchRepository, UserStockWatchRepository>();

        return services;
    }
}
