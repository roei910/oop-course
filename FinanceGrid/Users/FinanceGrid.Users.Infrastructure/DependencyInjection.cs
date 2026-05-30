using FinanceGrid.Users.Application.Interfaces;
using FinanceGrid.Users.Application.Services;
using FinanceGrid.Users.Domain.Interfaces;
using FinanceGrid.Users.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddUsersPersistence(configuration);

        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IShareService, ShareService>();

        return services;
    }
}
