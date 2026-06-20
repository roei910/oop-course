using UsersAbstractions.Dal;
using UsersAbstractions.Repositories;
using UsersAbstractions.Services;
using SharedLibrary.Models;
using SharedLibrary.Services;
using UsersService.Middleware;
using UsersService.Services;
using UsersService.Stubs;
using UsersLibrary.Repositories;
using UsersLibrary.Services;
using StocksApi.Providers.UsersSqlite;
using StocksApi.Providers.UsersSqlite.Dal;
using StocksAbstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UsersService
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
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IStockRepository, StubStockRepository>();

            var connectionString = _configuration.GetValue<string>(ConfigurationKeys.ConnectionStringSection)
                ?? "Data Source=Users.db";

            services.AddDbContextFactory<UsersSqliteDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddSingleton<IUsersDal, UsersDal>();
            services.AddSingleton<IUserStockWatchesDal, UserStockWatchesDal>();
            services.AddSingleton<IUserRepository, UserRepository>();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddLogging();
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var factory = app.ApplicationServices.GetRequiredService<IDbContextFactory<UsersSqliteDbContext>>();
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
