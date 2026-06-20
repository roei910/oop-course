using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

namespace FinanceGrid.Gateway.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            RemoveReverseProxyServices(services);

            services.AddReverseProxy()
                .LoadFromMemory(
                    new[]
                    {
                        new RouteConfig
                        {
                            RouteId = "stock-route",
                            ClusterId = "financialdata",
                            Match = new RouteMatch { Path = "/api/stock/{**catch-all}" }
                        },
                        new RouteConfig
                        {
                            RouteId = "user-route",
                            ClusterId = "users",
                            Match = new RouteMatch { Path = "/api/user/{**catch-all}" }
                        },
                        new RouteConfig
                        {
                            RouteId = "share-route",
                            ClusterId = "users",
                            Match = new RouteMatch { Path = "/api/share/{**catch-all}" }
                        },
                        new RouteConfig
                        {
                            RouteId = "userstockwatch-route",
                            ClusterId = "users",
                            Match = new RouteMatch { Path = "/api/userStockWatch/{**catch-all}" }
                        },
                        new RouteConfig
                        {
                            RouteId = "webhook-route",
                            ClusterId = "webhook",
                            Match = new RouteMatch { Path = "/api/webhook/{**catch-all}" }
                        }
                    },
                    new[]
                    {
                        new ClusterConfig
                        {
                            ClusterId = "financialdata",
                            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                            {
                                ["d1"] = new() { Address = "https://localhost:5001" }
                            }
                        },
                        new ClusterConfig
                        {
                            ClusterId = "users",
                            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                            {
                                ["d1"] = new() { Address = "https://localhost:5002" }
                            }
                        },
                        new ClusterConfig
                        {
                            ClusterId = "webhook",
                            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                            {
                                ["d1"] = new() { Address = "https://localhost:5003" }
                            }
                        }
                    });
        });
    }

    private static void RemoveReverseProxyServices(IServiceCollection services)
    {
        var descriptorsToRemove = services
            .Where(d =>
                d.ServiceType.FullName?.Contains("Yarp") == true ||
                d.ServiceType.FullName?.Contains("ReverseProxy") == true)
            .ToList();

        foreach (var descriptor in descriptorsToRemove)
            services.Remove(descriptor);
    }
}
