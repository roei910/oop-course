using Library.Interfaces;
using Library.Services;
using YahooFinance.Models;

namespace YahooFinance.Factories
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
            var webApi = new WebApi(apiConfiguration.BaseUrl, apiConfiguration.Headers);

            return webApi;
        }
    }
}