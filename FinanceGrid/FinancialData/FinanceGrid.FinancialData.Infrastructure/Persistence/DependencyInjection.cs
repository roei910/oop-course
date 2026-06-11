using FinanceGrid.FinancialData.Domain.Interfaces;
using FinanceGrid.FinancialData.Infrastructure.Persistence.Repositories;
using FinanceGrid.Shared.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FinanceGrid.FinancialData.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddFinancialDataPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration, "FinancialData");

        services.AddDbContextFactory<FinancialDataDbContext>((sp, options) =>
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

        services.AddSingleton<IStockRepository, StockRepository>();
        services.AddSingleton<ITrendRepository, TrendRepository>();
        services.AddSingleton<ISearchResultRepository, SearchResultRepository>();
        services.AddSingleton<IStockHistoryRepository, StockHistoryRepository>();

        return services;
    }
}
