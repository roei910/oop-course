using SharedLibrary.Models;
using StocksAbstractions.Dal;
using StocksAbstractions.Repositories;
using StocksAbstractions.Services;
using SharedLibrary.Services;
using StocksService.Middleware;
using StocksService.Services;
using StocksLibrary.Repositories;
using StocksLibrary.Services;
using StocksProvider;
using StocksProvider.Interfaces;
using StocksProvider.Services.FinanceApis;
using StocksProvider.Factories;
using StocksApi.Providers.Sqlite;
using StocksApi.Providers.Sqlite.Dal;
using Microsoft.EntityFrameworkCore;

namespace StocksService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddSingleton<IStockMarketTime, StockMarketTime>();
            services.AddSingleton<IStockNotificationSender, StubStockNotificationSender>();

            var connectionString = _configuration.GetValue<string>(ConfigurationKeys.ConnectionStringSection)
                ?? "Data Source=Stocks.db";

            services.AddDbContextFactory<SqliteDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddSingleton<IStocksDal, StocksDal>();
            services.AddSingleton<ITrendsDal, TrendsDal>();
            services.AddSingleton<ISearchResultsDal, SearchResultsDal>();
            services.AddSingleton<IStockHistoriesDal, StockHistoriesDal>();

            services.AddSingleton<IStockRepository, StockRepository>();
            services.AddSingleton<ITrendRepository, TrendRepository>();
            services.AddSingleton<ISearchResultRepository, SearchResultRepository>();

            services.AddSingleton<WebApiFactory>();

            services.AddSingleton<IFinanceStrategy, FinanceStrategy>();
            services.AddSingleton<List<IYahooFinance>>();
            services.AddSingleton<IYahooFinance, YahooFinance15>();
            services.AddSingleton<IYahooFinance, YahooFinance1>();

            services.AddSingleton<List<IStockAnalysisApi>>();
            services.AddSingleton<IStockAnalysisApi, YahooFinance127>();

            services.AddSingleton<IRealTimeFinanceData, RealTimeFinanceData>();

            services.AddHostedService<StocksAutomaticUpdater>();
            services.AddHostedService<StockAnalysisUpdater>();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddLogging();
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var factory = app.ApplicationServices.GetRequiredService<IDbContextFactory<SqliteDbContext>>();
            using (var context = factory.CreateDbContext())
                context.Database.Migrate();

            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger");
                    return;
                }

                await next();
            });
        }
    }
}
