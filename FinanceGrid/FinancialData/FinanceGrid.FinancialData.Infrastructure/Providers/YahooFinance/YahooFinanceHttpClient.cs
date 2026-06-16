using System.Text.Json;

namespace FinanceGrid.FinancialData.Infrastructure.Providers.YahooFinance;

public class YahooFinanceHttpClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public YahooFinanceHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string endpoint, params (string Key, string Value)[] queryParams)
    {
        if (_httpClient.BaseAddress is null)
            return default;

        var url = queryParams.Length == 0
            ? endpoint
            : $"{endpoint}?{string.Join("&", queryParams.Select(p => $"{p.Key}={p.Value}"))}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, s_jsonOptions);
    }
}
