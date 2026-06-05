namespace FinanceGrid.Users.Api
{
    using FinanceGrid.Users.Infrastructure;
    using FinanceGrid.Users.Api.Middleware;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            builder.Services.AddUsersInfrastructure(builder.Configuration);

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

            app.Run();
        }
    }
}
