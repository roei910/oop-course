using Newtonsoft.Json.Linq;
using RestSharp;
using YahooFinance.Interfaces;

namespace YahooFinance.Services
{
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
            var request = GetRestRequest(Method.Get, endPoint, queryParams);

            var response = await _client.ExecuteAsync(request);

            if (response.StatusDescription == "Too Many Requests")
                throw new Exception($"Too many requests for server");

            var content = response.Content ??
                throw new Exception("couldnt get the response from server");

            try
            {
                var jObject = JObject.Parse(content);
                var deserialized = jObject.ToObject<T>();

                return deserialized;
            }
            catch(Exception ex)
            {
                throw new Exception($"couldn't deserialize the response to the type, {typeof(T)}", ex);
            }
        }

        public async Task<RestResponse> GetResponseAsync(string endPoint,
            params KeyValuePair<string, string>[] queryParams)
        {
            var request = GetRestRequest(Method.Get, endPoint, queryParams);

            var response = await _client.ExecuteAsync(request) ??
                throw new Exception("couldnt get the response from server");

            return response;
        }

        private RestRequest GetRestRequest(Method method, string endPoint,
            params KeyValuePair<string, string>[] queryParams)
        {
            var request = new RestRequest(endPoint, method);
            
            queryParams.ToList().ForEach(queryParam =>
                request.AddParameter(queryParam.Key, queryParam.Value));

            return request;
        }
    }
}