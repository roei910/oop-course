using SharedLibrary.Services;
using StocksProvider.Models;

namespace StocksProvider.Factories
{
    public class WebApiFactory
    {
        private readonly IAppConfiguration _financeConfiguration;

        public WebApiFactory(IAppConfiguration financeConfiguration)
        {
            _financeConfiguration = financeConfiguration;
        }

        public IWebApi Generate(string section)
        {
            var apiConfiguration = _financeConfiguration.Get<ApiConfiguration>(section);
            var webApiType = typeof(SharedLibrary.Services.WebApi);
            var constructor = webApiType.GetConstructor(
                new[] { typeof(string), typeof(Dictionary<string, string>) })!;
            var webApi = (IWebApi)constructor.Invoke(
                new object[] { apiConfiguration.BaseUrl!, apiConfiguration.Headers! });

            return webApi;
        }
    }
}