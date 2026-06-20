namespace FinanceGrid.FinancialData.Api
{
    using FinanceGrid.FinancialData.Infrastructure;
    using FinanceGrid.FinancialData.Infrastructure.Persistence;
    using FinanceGrid.FinancialData.Api.Middleware;
    using FinanceGrid.Persistence;

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

            builder.Services.AddFinancialDataServices(builder.Configuration);

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

            app.Services.ApplyMigrations<FinancialDataDbContext>();
            app.Run();
        }
    }
}
