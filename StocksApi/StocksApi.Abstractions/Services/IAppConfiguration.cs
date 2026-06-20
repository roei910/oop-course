namespace StocksApi.Abstractions.Services
{
	public interface IAppConfiguration
	{
        T Get<T>(string section);
        T Get<T>(params string[] sections);
    }
}
