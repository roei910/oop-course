namespace FinanceGrid.FinancialData.Api
{
    using FinanceGrid.FinancialData.Infrastructure;
    using FinanceGrid.FinancialData.Infrastructure.Persistence;
    using FinanceGrid.FinancialData.Api.Middleware;
    using FinanceGrid.Shared.Database;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults(typeof(Program).Assembly);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            builder.Services.AddFinancialDataInfrastructure(builder.Configuration);

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.MapControllers();
            app.MapHealthChecks("/health");

            app.ApplyMigrations();
            app.Run();
        }
    }

    internal static class WebApplicationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbConfig = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;
            var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FinancialDataDbContext>>();
            using var context = contextFactory.CreateDbContext();
            if (dbConfig.IsPostgreSQL)
            {
                context.Database.Migrate();
            }
            else
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
