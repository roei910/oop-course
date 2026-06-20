namespace SharedLibrary.Services
{
    public interface IWebApi
    {
        Task<T?> GetResponseAsync<T>(string endpoint,
            params KeyValuePair<string, string>[] queryParams);
    }
}
