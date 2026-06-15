using Newtonsoft.Json.Linq;
using RestSharp;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class WebApi : IWebApi
{
    private readonly RestClient _client;

    public WebApi(string baseUrl, Dictionary<string, string> headers)
    {
        _client = new RestClient(baseUrl);
        _client.AddDefaultHeaders(headers);
    }

    public async Task<T?> GetResponseAsync<T>(string endPoint,
        params KeyValuePair<string, string>[] queryParams)
    {
        var request = new RestRequest(endPoint, Method.Get);

        foreach (var param in queryParams)
            request.AddParameter(param.Key, param.Value);

        var response = await _client.ExecuteAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            throw new Exception("Too many requests for server");

        var content = response.Content ??
            throw new Exception("Could not get the response from server");

        var jObject = JObject.Parse(content);
        return jObject.ToObject<T>();
    }
}
