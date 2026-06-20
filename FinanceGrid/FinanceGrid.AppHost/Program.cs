using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SQLite";

var messaging = builder.AddRabbitMQ("messaging");

var financialDataApi = builder.AddProject<Projects.FinanceGrid_FinancialData_Api>("financialdata-api")
    .WithEnvironment("Database__Provider", databaseProvider);

var usersApi = builder.AddProject<Projects.FinanceGrid_Users_Api>("users-api")
    .WithEnvironment("Database__Provider", databaseProvider);

var webhookApi = builder.AddProject<Projects.FinanceGrid_Webhook_Api>("webhook-api")
    .WithEnvironment("Database__Provider", databaseProvider);

var webhookProcessing = builder.AddProject<Projects.FinanceGrid_Webhook_Processing>("webhook-processing")
    .WithEnvironment("Database__Provider", databaseProvider);

var gateway = builder.AddProject<Projects.FinanceGrid_Gateway>("gateway");

switch (databaseProvider.ToLower())
{
    case "postgresql":
        var postgres = builder.AddPostgres("postgres");

        financialDataApi
            .WithReference(postgres.AddDatabase("FinancialData"))
            .WithEnvironment("Database__Provider", "PostgreSQL");
        usersApi
            .WithReference(postgres.AddDatabase("Users"))
            .WithEnvironment("Database__Provider", "PostgreSQL");

        var webhookDb = postgres.AddDatabase("Webhook");
        webhookApi
            .WithReference(webhookDb)
            .WithEnvironment("Database__Provider", "PostgreSQL");
        webhookProcessing
            .WithReference(webhookDb)
            .WithEnvironment("Database__Provider", "PostgreSQL");
        break;
    case "sqlite":
    default:
        break;
}

financialDataApi.WithReference(messaging);
webhookApi.WithReference(messaging);
webhookProcessing.WithReference(messaging);

gateway
    .WithReference(financialDataApi)
    .WithReference(usersApi)
    .WithReference(webhookApi);

builder.Build().Run();
