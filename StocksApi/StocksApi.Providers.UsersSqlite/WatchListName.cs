namespace StocksApi.Providers.UsersSqlite
{
    public class WatchListName
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserEmail { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
