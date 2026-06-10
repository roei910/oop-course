using FinanceGrid.FinancialData.Domain.Interfaces;
using FinanceGrid.FinancialData.Infrastructure.Persistence.Repositories;
using FinanceGrid.Shared.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceGrid.FinancialData.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddFinancialDataPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var dbConfig = configuration.GetDatabaseConfiguration("FinancialData");

        services.AddDbContextFactory<FinancialDataDbContext>(options =>
        {
            if (dbConfig.IsPostgreSQL)
                options.UseNpgsql(dbConfig.ConnectionString);
            else
                options.UseSqlite(dbConfig.ConnectionString);
        });

        services.AddSingleton<IStockRepository, StockRepository>();
        services.AddSingleton<ITrendRepository, TrendRepository>();
        services.AddSingleton<ISearchResultRepository, SearchResultRepository>();
        services.AddSingleton<IStockHistoryRepository, StockHistoryRepository>();

        return services;
    }
}
