using FinanceGrid.FinancialData.Domain.Interfaces;
using FinanceGrid.FinancialData.Infrastructure.Persistence.Repositories;
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
        var provider = configuration.GetValue<string>("DatabaseProvider") ?? "SQLite";
        var connectionString = configuration.GetConnectionString("FinancialData")
            ?? "Data Source=FinancialData.db";

        services.AddDbContextFactory<FinancialDataDbContext>(options =>
        {
            if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
                options.UseNpgsql(connectionString);
            else
                options.UseSqlite(connectionString);
        });

        services.AddSingleton<IStockRepository, StockRepository>();
        services.AddSingleton<ITrendRepository, TrendRepository>();
        services.AddSingleton<ISearchResultRepository, SearchResultRepository>();
        services.AddSingleton<IStockHistoryRepository, StockHistoryRepository>();

        return services;
    }
}
