using SharedLibrary.Models;
using StocksService;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;

        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();

        var secretsFilePath = Environment.GetEnvironmentVariable(ConfigurationKeys.SecretsFilePathKey);
        if (!string.IsNullOrEmpty(secretsFilePath))
            config.AddJsonFile(secretsFilePath, optional: false, reloadOnChange: true);
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .Run();
