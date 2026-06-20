using FinanceGrid.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceGrid.Users.Infrastructure.Persistence;

public class UsersDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserStockWatch> UserStockWatches => Set<UserStockWatch>();

    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Ignore(e => e.StockNotifications);
            entity.Ignore(e => e.UserStockNotesBySymbol);
            entity.Ignore(e => e.WatchListNames);
        });

        modelBuilder.Entity<UserStockWatch>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new { e.UserEmail, e.ListName, e.StockSymbol }).IsUnique();
            entity.Ignore(e => e.PurchaseGuidToShares);
        });
    }
}
