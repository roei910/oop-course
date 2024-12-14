using RestSharp;

namespace YahooFinance.Interfaces
{
    public interface IWebApi
    {
        Task<T?> GetResponseAsync<T>(string endpoint,
            params KeyValuePair<string, string>[] queryParams);

        Task<RestResponse> GetResponseAsync(string endPoint,
            params KeyValuePair<string, string>[] queryParams);
    }
}