using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.FinancialData.Infrastructure.Providers;

public static class DependencyInjection
{
    public static IServiceCollection AddFinancialDataProviders(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<WebApiFactory>();

        services.AddSingleton<IYahooFinance, YahooFinance15>();
        services.AddSingleton<IYahooFinance, YahooFinance1>();

        services.AddSingleton<IStockAnalysisApi, YahooFinance127>();

        services.AddSingleton<IRealTimeFinanceData, RealTimeFinanceData>();

        services.AddSingleton<IFinanceStrategy, FinanceStrategy>();

        return services;
    }
}
