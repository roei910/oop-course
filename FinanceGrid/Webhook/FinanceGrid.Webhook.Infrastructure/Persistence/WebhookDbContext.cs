using FinanceGrid.Webhook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceGrid.Webhook.Infrastructure.Persistence;

public class WebhookDbContext : DbContext
{
    public DbSet<WebhookSubscription> Subscriptions => Set<WebhookSubscription>();
    public DbSet<WebhookDeliveryAttempt> DeliveryAttempts => Set<WebhookDeliveryAttempt>();

    public WebhookDbContext(DbContextOptions<WebhookDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WebhookSubscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserEmail);
            entity.HasIndex(e => new { e.EventType, e.IsActive });
        });

        modelBuilder.Entity<WebhookDeliveryAttempt>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SubscriptionId);
            entity.HasOne(e => e.Subscription)
                .WithMany()
                .HasForeignKey(e => e.SubscriptionId);
        });
    }
}
