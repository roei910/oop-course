using Library.Models;
using StocksApi;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;

        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();

        if (env.IsProduction())
        {
            var secretsFilePath = Environment.GetEnvironmentVariable(ConfigurationKeys.SecretsFilePathKey);
            config.AddJsonFile(secretsFilePath!, optional: false, reloadOnChange: true);
        }
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .Run();