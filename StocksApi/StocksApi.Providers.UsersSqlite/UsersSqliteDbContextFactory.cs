using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StocksApi.Providers.UsersSqlite
{
    public class UsersSqliteDbContextFactory : IDesignTimeDbContextFactory<UsersSqliteDbContext>
    {
        public UsersSqliteDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersSqliteDbContext>();
            optionsBuilder.UseSqlite("Data Source=Users.db");
            return new UsersSqliteDbContext(optionsBuilder.Options);
        }
    }
}
