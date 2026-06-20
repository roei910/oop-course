using FinanceGrid.FinancialData.Domain.Interfaces;
using FinanceGrid.FinancialData.Infrastructure.Persistence.Repositories;
using FinanceGrid.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.FinancialData.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddFinancialDataDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<FinancialDataDbContext>(configuration, "FinancialData");

        services.AddSingleton<IStockRepository, StockRepository>();
        services.AddSingleton<ITrendRepository, TrendRepository>();
        services.AddSingleton<ISearchResultRepository, SearchResultRepository>();
        services.AddSingleton<IStockHistoryRepository, StockHistoryRepository>();

        return services;
    }
}
