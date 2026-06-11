using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;
using FinanceGrid.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.FinancialData.Infrastructure.Providers;

public static class DependencyInjection
{
    public static IServiceCollection AddFinancialDataProviders(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IFinanceStrategy, FinanceStrategy>();

        return services;
    }
}
