using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StocksApi.Providers.Sqlite
{
    public class SqliteDbContextFactory : IDesignTimeDbContextFactory<SqliteDbContext>
    {
        public SqliteDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqliteDbContext>();
            optionsBuilder.UseSqlite("Data Source=StocksApi.db");
            return new SqliteDbContext(optionsBuilder.Options);
        }
    }
}
