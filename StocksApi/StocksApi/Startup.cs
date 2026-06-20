using StocksAbstractions.Dal;
using UsersAbstractions.Dal;
using SharedLibrary.Models;
using StocksAbstractions.Repositories;
using UsersAbstractions.Repositories;
using StocksAbstractions.Services;
using UsersAbstractions.Services;
using SharedLibrary.Services;
using StocksApi.Middleware;
using StocksApi.Services;
using StocksApi.Providers.Mongo;
using StocksLibrary.Repositories;
using UsersLibrary.Repositories;
using StocksLibrary.Services;
using UsersLibrary.Services;
using StocksProvider;
using StocksProvider.Interfaces;
using StocksProvider.Services.FinanceApis;
using StocksProvider.Factories;
using Microsoft.EntityFrameworkCore;

namespace StocksApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly string _databaseProvider;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseProvider = _configuration.GetValue<string>(ConfigurationKeys.DatabaseProviderSection) ?? "Mongo";
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
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IStockNotificationSender, StockNotificationSender>();

            var databaseProvider = _databaseProvider;

            if (string.Equals(databaseProvider, "Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                RegisterSqliteProvider(services);
            }
            else
            {
                RegisterMongoProvider(services);
            }

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
            services.AddHealthChecks();
        }

        private void RegisterMongoProvider(IServiceCollection services)
        {
            services.AddSingleton<IMongoDbContext, MongoDbContext>();
            services.AddSingleton<IStocksDal, Providers.Mongo.Dal.StocksDal>();
            services.AddSingleton<IUsersDal, Providers.Mongo.Dal.UsersDal>();
            services.AddSingleton<IUserStockWatchesDal, Providers.Mongo.Dal.UserStockWatchesDal>();
            services.AddSingleton<ITrendsDal, Providers.Mongo.Dal.TrendsDal>();
            services.AddSingleton<ISearchResultsDal, Providers.Mongo.Dal.SearchResultsDal>();
            services.AddSingleton<IStockHistoriesDal, Providers.Mongo.Dal.StockHistoriesDal>();
        }

        private void RegisterSqliteProvider(IServiceCollection services)
        {
            var connectionString = _configuration.GetValue<string>(ConfigurationKeys.ConnectionStringSection)
                ?? "Data Source=StocksApi.db";

            services.AddDbContextFactory<Providers.Sqlite.SqliteDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddSingleton<IStocksDal, Providers.Sqlite.Dal.StocksDal>();
            services.AddSingleton<ITrendsDal, Providers.Sqlite.Dal.TrendsDal>();
            services.AddSingleton<ISearchResultsDal, Providers.Sqlite.Dal.SearchResultsDal>();
            services.AddSingleton<IStockHistoriesDal, Providers.Sqlite.Dal.StockHistoriesDal>();

            var usersConnectionString = _configuration.GetValue<string>(ConfigurationKeys.ConnectionStringSection)
                ?? "Data Source=Users.db";

            services.AddDbContextFactory<Providers.UsersSqlite.UsersSqliteDbContext>(options =>
                options.UseSqlite(usersConnectionString));

            services.AddSingleton<IUsersDal, Providers.UsersSqlite.Dal.UsersDal>();
            services.AddSingleton<IUserStockWatchesDal, Providers.UsersSqlite.Dal.UserStockWatchesDal>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (string.Equals(_databaseProvider, "Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                var stocksFactory = app.ApplicationServices.GetRequiredService<IDbContextFactory<Providers.Sqlite.SqliteDbContext>>();
                using (var context = stocksFactory.CreateDbContext())
                    context.Database.Migrate();

                var usersFactory = app.ApplicationServices.GetRequiredService<IDbContextFactory<Providers.UsersSqlite.UsersSqliteDbContext>>();
                using (var context = usersFactory.CreateDbContext())
                    context.Database.Migrate();
            }

            app.UseMiddleware<ExceptionMiddleware>();

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
