var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("messaging");

var financialDataApi = builder.AddProject<Projects.FinanceGrid_FinancialData_Api>("financialdata-api");
var usersApi = builder.AddProject<Projects.FinanceGrid_Users_Api>("users-api");
var webhookApi = builder.AddProject<Projects.FinanceGrid_Webhook_Api>("webhook-api");
var webhookProcessing = builder.AddProject<Projects.FinanceGrid_Webhook_Processing>("webhook-processing");
var gateway = builder.AddProject<Projects.FinanceGrid_Gateway>("gateway");

if (builder.Environment.EnvironmentName != "Development")
{
    var postgres = builder.AddPostgres("postgres");

    financialDataApi
        .WithReference(postgres.AddDatabase("FinancialData"))
        .WithEnvironment("DatabaseProvider", "PostgreSQL");

    usersApi
        .WithReference(postgres.AddDatabase("Users"))
        .WithEnvironment("DatabaseProvider", "PostgreSQL");

    var webhookDb = postgres.AddDatabase("Webhook");

    webhookApi
        .WithReference(webhookDb)
        .WithEnvironment("DatabaseProvider", "PostgreSQL");

    webhookProcessing
        .WithReference(webhookDb)
        .WithEnvironment("DatabaseProvider", "PostgreSQL");
}

financialDataApi.WithReference(messaging);
webhookApi.WithReference(messaging);
webhookProcessing.WithReference(messaging);

gateway
    .WithReference(financialDataApi)
    .WithReference(usersApi)
    .WithReference(webhookApi);

builder.Build().Run();