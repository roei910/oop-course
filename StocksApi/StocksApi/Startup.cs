using StocksApi.Dal;
using StocksApi.Interfaces;
using StocksApi.Repositories;
using StocksApi.Services;
using YahooFinance;
using YahooFinance.Interfaces;
using YahooFinance.Services.FinanceApis;
using YahooFinance.Factories;
using Library.Interfaces;

namespace StocksApi
{
    public class Startup
    {
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
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IStockNotificationSender, StockNotificationSender>();

            services.AddSingleton<IStocksDal, StocksDal>();
            services.AddSingleton<IUsersDal, UsersDal>();
            services.AddSingleton<ITrendsDal, TrendsDal>();
            services.AddSingleton<ISearchResultsDal, SearchResultsDal>();

            services.AddSingleton<IStockRepository, StockRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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