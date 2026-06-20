using Microsoft.EntityFrameworkCore;
using UsersAbstractions.Models;
using UsersAbstractions.Models.Notifications;
using UsersAbstractions.Models.Shares;
using UsersAbstractions.Models.StockNotes;

namespace StocksApi.Providers.UsersSqlite
{
    public class UsersSqliteDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<StockNotification> StockNotifications => Set<StockNotification>();
        public DbSet<UserStockNote> UserStockNotes => Set<UserStockNote>();
        public DbSet<UserStockWatch> UserStockWatches => Set<UserStockWatch>();
        public DbSet<Share> Shares => Set<Share>();
        public DbSet<WatchListName> WatchListNames => Set<WatchListName>();

        public UsersSqliteDbContext(DbContextOptions<UsersSqliteDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockNotification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne<User>()
                    .WithMany(u => u.StockNotifications)
                    .HasForeignKey("UserEmail")
                    .HasPrincipalKey(e => e.Email);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Ignore(e => e.WatchListNames);
                entity.Ignore(e => e.UserStockNotesBySymbol);
            });

            modelBuilder.Entity<WatchListName>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => new { e.UserEmail, e.Name }).IsUnique();

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserEmail)
                    .HasPrincipalKey(e => e.Email);
            });

            modelBuilder.Entity<UserStockNote>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property<string>("UserEmail");
                entity.Property<string>("StockSymbol");
                entity.HasIndex("UserEmail", "StockSymbol");

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserEmail")
                    .HasPrincipalKey(e => e.Email);
            });

            modelBuilder.Entity<UserStockWatch>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.UserEmail);
                entity.HasIndex(e => new { e.UserEmail, e.ListName });

                entity.Ignore(e => e.PurchaseGuidToShares);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserEmail)
                    .HasPrincipalKey(e => e.Email);
            });

            modelBuilder.Entity<Share>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property<string>("UserStockWatchId");
                entity.Property<string>("PurchaseGuid");
                entity.HasIndex("PurchaseGuid").IsUnique();
                entity.HasIndex("UserStockWatchId");

                entity.HasOne<UserStockWatch>()
                    .WithMany()
                    .HasForeignKey("UserStockWatchId");
            });
        }
    }
}
