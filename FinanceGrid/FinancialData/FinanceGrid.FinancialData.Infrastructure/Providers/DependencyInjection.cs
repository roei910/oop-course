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
        ConfigureNamedHttpClient(services, configuration, "YahooFinance1");
        ConfigureNamedHttpClient(services, configuration, "YahooFinance15");
        ConfigureNamedHttpClient(services, configuration, "YahooFinance127");
        ConfigureNamedHttpClient(services, configuration, "RealTimeFinanceData");

        services.AddTransient<YahooFinance1>();
        services.AddTransient<YahooFinance15>();
        services.AddTransient<YahooFinance127>();
        services.AddTransient<RealTimeFinanceData>();

        services.AddSingleton<IYahooFinance>(sp => sp.GetRequiredService<YahooFinance15>());
        services.AddSingleton<IYahooFinance>(sp => sp.GetRequiredService<YahooFinance1>());
        services.AddSingleton<IYahooFinanceBulk>(sp => sp.GetRequiredService<YahooFinance15>());
        services.AddSingleton<IYahooFinanceBulk>(sp => sp.GetRequiredService<YahooFinance1>());
        services.AddSingleton<IStockAnalysisApi>(sp => sp.GetRequiredService<YahooFinance127>());
        services.AddSingleton<IRealTimeFinanceData>(sp => sp.GetRequiredService<RealTimeFinanceData>());

        services.AddSingleton<IFinanceStrategy, FinanceStrategy>();

        return services;
    }

    private static void ConfigureNamedHttpClient(
        IServiceCollection services,
        IConfiguration configuration,
        string name)
    {
        var section = configuration.GetSection(name);
        if (!section.Exists())
        {
            services.AddHttpClient(name);
            return;
        }

        services.AddHttpClient(name)
            .ConfigureHttpClient((sp, client) =>
            {
                var config = section.Get<ApiConfiguration>();
                if (config is null) return;

                client.BaseAddress = new Uri(config.BaseUrl);
                foreach (var header in config.Headers)
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            });
    }
}
