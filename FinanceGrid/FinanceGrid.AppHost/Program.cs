var builder = DistributedApplication.CreateBuilder(args);

var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SQLite";
var isPostgres = databaseProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase);

var messaging = builder.AddRabbitMQ("messaging");

var financialDataApi = builder.AddProject<Projects.FinanceGrid_FinancialData_Api>("financialdata-api")
    .WithEnvironment("DatabaseProvider", databaseProvider);

var usersApi = builder.AddProject<Projects.FinanceGrid_Users_Api>("users-api")
    .WithEnvironment("DatabaseProvider", databaseProvider);

var webhookApi = builder.AddProject<Projects.FinanceGrid_Webhook_Api>("webhook-api")
    .WithEnvironment("DatabaseProvider", databaseProvider);

var webhookProcessing = builder.AddProject<Projects.FinanceGrid_Webhook_Processing>("webhook-processing")
    .WithEnvironment("DatabaseProvider", databaseProvider);

var gateway = builder.AddProject<Projects.FinanceGrid_Gateway>("gateway");

if (isPostgres)
{
    var postgres = builder.AddPostgres("postgres");

    financialDataApi.WithReference(postgres.AddDatabase("FinancialData"));
    usersApi.WithReference(postgres.AddDatabase("Users"));

    var webhookDb = postgres.AddDatabase("Webhook");
    webhookApi.WithReference(webhookDb);
    webhookProcessing.WithReference(webhookDb);
}

financialDataApi.WithReference(messaging);
webhookApi.WithReference(messaging);
webhookProcessing.WithReference(messaging);

gateway
    .WithReference(financialDataApi)
    .WithReference(usersApi)
    .WithReference(webhookApi);

builder.Build().Run();