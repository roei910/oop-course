using FinanceGrid.Persistence;
using FinanceGrid.Users.Domain.Interfaces;
using FinanceGrid.Users.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.Users.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>(configuration, "Users");

        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserStockWatchRepository, UserStockWatchRepository>();

        return services;
    }
}
