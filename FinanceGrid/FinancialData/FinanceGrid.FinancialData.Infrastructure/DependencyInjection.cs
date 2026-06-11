using FinanceGrid.FinancialData.Application.Interfaces;
using FinanceGrid.FinancialData.Application.Services;
using FinanceGrid.FinancialData.Domain.Interfaces;
using FinanceGrid.FinancialData.Infrastructure.BackgroundServices;
using FinanceGrid.FinancialData.Infrastructure.Messaging;
using FinanceGrid.FinancialData.Infrastructure.Persistence;
using FinanceGrid.FinancialData.Infrastructure.Providers;
using FinanceGrid.FinancialData.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.FinancialData.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFinancialDataServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFinancialDataDatabase(configuration);
        services.AddFinancialDataProviders(configuration);

        services.AddSingleton<IStockService, StockService>();
        services.AddSingleton<ITrendService, TrendService>();
        services.AddSingleton<ISearchService, SearchService>();

        services.AddSingleton<IStockMarketTime, StockMarketTimeService>();
        services.AddSingleton<IStockNotificationSender, StockNotificationSender>();

        services.AddHostedService<StocksAutomaticUpdater>();
        services.AddHostedService<StockAnalysisUpdater>();

        services.AddHttpClient();

        return services;
    }
}
