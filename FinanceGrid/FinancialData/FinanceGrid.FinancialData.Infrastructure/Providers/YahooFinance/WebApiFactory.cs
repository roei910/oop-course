using Microsoft.Extensions.Configuration;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class WebApiFactory
{
    private readonly IConfiguration _configuration;

    public WebApiFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IWebApi Generate(string section)
    {
        var config = _configuration.GetRequiredSection(section).Get<ApiConfiguration>()!;
        return new WebApi(config.BaseUrl, config.Headers);
    }
}
