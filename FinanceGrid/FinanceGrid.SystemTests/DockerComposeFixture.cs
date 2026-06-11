using System.Diagnostics;
using Xunit;

namespace FinanceGrid.SystemTests;

public class DockerComposeFixture : IAsyncLifetime
{
    public const string FinancialDataBaseUrl = "http://localhost:5001";
    public const string UsersBaseUrl = "http://localhost:5002";
    public const string WebhookBaseUrl = "http://localhost:5003";
    public const string GatewayBaseUrl = "http://localhost:5000";

    private const string ComposeFile = "docker-compose.test.yml";
    private const int StartupTimeoutSeconds = 90;
    private const int HealthCheckTimeoutSeconds = 60;

    private readonly bool _manageDocker = true;
    private readonly string _composeProjectName = $"financegrid-tests-{Guid.NewGuid():N}";

    public bool ServicesAreRunning { get; private set; }

    public DockerComposeFixture()
    {
        var env = Environment.GetEnvironmentVariable("MANAGE_DOCKER_COMPOSE");
        if (!string.IsNullOrEmpty(env) && bool.TryParse(env, out var managed))
        {
            _manageDocker = managed;
        }
    }

    public async Task InitializeAsync()
    {
        if (!_manageDocker)
        {
            // Caller is expected to have the stack running already; just verify
            ServicesAreRunning = await AreServicesHealthyAsync();
            return;
        }

        var upOk = await RunComposeAsync($"-p {_composeProjectName} -f {ComposeFile} up -d --build");
        if (!upOk)
        {
            throw new InvalidOperationException("docker compose up failed. See output above.");
        }

        ServicesAreRunning = await AreServicesHealthyAsync();
        if (!ServicesAreRunning)
        {
            await RunComposeAsync($"-p {_composeProjectName} -f {ComposeFile} down -v");
            throw new InvalidOperationException(
                "Services did not become healthy within the timeout. " +
                "Check 'docker compose logs' for details.");
        }
    }

    public async Task DisposeAsync()
    {
        if (!_manageDocker) return;
        await RunComposeAsync($"-p {_composeProjectName} -f {ComposeFile} down -v");
    }

    private async Task<bool> AreServicesHealthyAsync()
    {
        var endpoints = new[]
        {
            $"{FinancialDataBaseUrl}/health",
            $"{UsersBaseUrl}/health",
            $"{WebhookBaseUrl}/health"
        };

        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
        var deadline = DateTime.UtcNow.AddSeconds(HealthCheckTimeoutSeconds);

        while (DateTime.UtcNow < deadline)
        {
            var allHealthy = true;
            foreach (var url in endpoints)
            {
                try
                {
                    var resp = await http.GetAsync(url);
                    if (!resp.IsSuccessStatusCode)
                    {
                        allHealthy = false;
                        break;
                    }
                }
                catch
                {
                    allHealthy = false;
                    break;
                }
            }

            if (allHealthy) return true;
            await Task.Delay(2000);
        }

        return false;
    }

    private static async Task<bool> RunComposeAsync(string args)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"compose {args}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using var proc = Process.Start(psi)!;
            var stdout = await proc.StandardOutput.ReadToEndAsync();
            var stderr = await proc.StandardError.ReadToEndAsync();
            await proc.WaitForExitAsync();

            if (!string.IsNullOrWhiteSpace(stdout))
                Console.WriteLine($"[docker] {stdout}");
            if (!string.IsNullOrWhiteSpace(stderr))
                Console.Error.WriteLine($"[docker] {stderr}");

            return proc.ExitCode == 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[docker] Failed to run docker compose: {ex.Message}");
            return false;
        }
    }
}

[CollectionDefinition("DockerCompose")]
public class DockerComposeCollection : ICollectionFixture<DockerComposeFixture>
{
}
