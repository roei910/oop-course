using FinanceGrid.FinancialData.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceGrid.FinancialData.Infrastructure.Persistence;

public class FinancialDataDbContext : DbContext
{
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<StockHistoryEntry> StockHistory => Set<StockHistoryEntry>();
    public DbSet<MarketTrend> MarketTrends => Set<MarketTrend>();
    public DbSet<StockTrend> StockTrends => Set<StockTrend>();
    public DbSet<StockNews> StockNews => Set<StockNews>();
    public DbSet<SearchResult> SearchResults => Set<SearchResult>();
    public DbSet<StockSearchResult> StockSearchResults => Set<StockSearchResult>();

    public FinancialDataDbContext(DbContextOptions<FinancialDataDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Symbol).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.OwnsOne(e => e.Analysis, analysis =>
            {
                analysis.Property(a => a.Symbol).HasColumnName("AnalysisSymbol");
                analysis.Property(a => a.UpdatedTime).HasColumnName("AnalysisUpdatedTime");
                analysis.Property(a => a.TargetHighPrice).HasColumnName("AnalysisTargetHighPrice");
                analysis.Property(a => a.TargetLowPrice).HasColumnName("AnalysisTargetLowPrice");
                analysis.Property(a => a.TargetMeanPrice).HasColumnName("AnalysisTargetMeanPrice");
                analysis.Property(a => a.TargetMedianPrice).HasColumnName("AnalysisTargetMedianPrice");
            });

            entity.Ignore(e => e.StockNotifications);
        });

        modelBuilder.Entity<StockHistoryEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => e.StockSymbol);
            entity.HasIndex(e => new { e.StockSymbol, e.Date });
        });

        modelBuilder.Entity<MarketTrend>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => e.TrendName).IsUnique();
        });

        modelBuilder.Entity<StockTrend>(entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd();
            entity.HasKey("Id");
            entity.HasOne<MarketTrend>()
                .WithMany(m => m.TrendingStocks)
                .HasForeignKey("MarketTrendId");
        });

        modelBuilder.Entity<StockNews>(entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd();
            entity.HasKey("Id");
            entity.HasOne<MarketTrend>()
                .WithMany(m => m.StockNewsItems)
                .HasForeignKey("MarketTrendId");
            entity.HasMany(e => e.StocksInNews)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "StockNewsStockTrend",
                    j => j.HasOne<StockTrend>().WithMany().HasForeignKey("StockTrendId"),
                    j => j.HasOne<StockNews>().WithMany().HasForeignKey("StockNewsId"),
                    j => j.HasKey("StockNewsId", "StockTrendId"));
        });

        modelBuilder.Entity<SearchResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => e.SearchTerm).IsUnique();
        });

        modelBuilder.Entity<StockSearchResult>(entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd();
            entity.HasKey("Id");
            entity.HasOne<SearchResult>()
                .WithMany(s => s.StockSearchResults)
                .HasForeignKey("SearchResultId");
        });
    }
}
