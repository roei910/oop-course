namespace FinanceGrid.Shared.Tests;

public class StockPriceUpdatedEventTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var before = DateTime.UtcNow;

        var evt = new StockPriceUpdatedEvent("AAPL", 150.00m, 155.50m, 3.67m);

        var after = DateTime.UtcNow;

        Assert.Equal("AAPL", evt.Symbol);
        Assert.Equal(150.00m, evt.OldPrice);
        Assert.Equal(155.50m, evt.NewPrice);
        Assert.Equal(3.67m, evt.ChangePercent);
        Assert.InRange(evt.UpdatedAt, before, after);
        Assert.InRange(evt.OccurredOn, before, after);
    }

    [Fact]
    public void InheritsFromDomainEvent()
    {
        var evt = new StockPriceUpdatedEvent("TSLA", 200m, 205m, 2.5m);

        Assert.IsAssignableFrom<DomainEvent>(evt);
        Assert.IsAssignableFrom<IDomainEvent>(evt);
    }

    [Fact]
    public void UpdatedAt_Kind_IsUtc()
    {
        var evt = new StockPriceUpdatedEvent("GOOG", 100m, 105m, 5m);

        Assert.Equal(DateTimeKind.Utc, evt.UpdatedAt.Kind);
    }

    [Fact]
    public void Properties_AreImmutable()
    {
        var evt = new StockPriceUpdatedEvent("MSFT", 400m, 410m, 2.5m);

        Assert.Equal("MSFT", evt.Symbol);
        Assert.Equal(400m, evt.OldPrice);
        Assert.Equal(410m, evt.NewPrice);

        var propSymbol = typeof(StockPriceUpdatedEvent).GetProperty("Symbol")!;
        Assert.False(propSymbol.CanWrite);
    }

    [Fact]
    public void ZeroChangePercent_Works()
    {
        var evt = new StockPriceUpdatedEvent("AAPL", 100m, 100m, 0m);

        Assert.Equal(0m, evt.ChangePercent);
        Assert.Equal(evt.OldPrice, evt.NewPrice);
    }

    [Fact]
    public void NegativeChangePercent_Works()
    {
        var evt = new StockPriceUpdatedEvent("AAPL", 100m, 95m, -5m);

        Assert.Equal(-5m, evt.ChangePercent);
        Assert.True(evt.NewPrice < evt.OldPrice);
    }
}
