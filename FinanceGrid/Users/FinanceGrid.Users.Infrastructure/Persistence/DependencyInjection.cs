using FinanceGrid.Shared.Database;
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
        var dbConfig = configuration.GetDatabaseConfiguration("Users");

        services.AddDbContextFactory<UsersDbContext>(options =>
        {
            if (dbConfig.IsPostgreSQL)
                options.UseNpgsql(dbConfig.ConnectionString);
            else
                options.UseSqlite(dbConfig.ConnectionString);
        });

        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserStockWatchRepository, UserStockWatchRepository>();

        return services;
    }
}
