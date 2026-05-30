using FinanceGrid.Users.Domain.Interfaces;
using FinanceGrid.Users.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.Users.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var provider = configuration.GetValue<string>("DatabaseProvider") ?? "SQLite";
        var connectionString = configuration.GetConnectionString("Users")
            ?? "Data Source=Users.db";

        services.AddDbContextFactory<UsersDbContext>(options =>
        {
            if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
                options.UseNpgsql(connectionString);
            else
                options.UseSqlite(connectionString);
        });

        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserStockWatchRepository, UserStockWatchRepository>();

        return services;
    }
}
