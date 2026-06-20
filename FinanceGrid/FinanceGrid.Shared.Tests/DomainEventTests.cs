namespace FinanceGrid.Shared.Tests;

public class DomainEventTests
{
    [Fact]
    public void OccurredOn_IsSetToUtcNow_AtConstruction()
    {
        var before = DateTime.UtcNow;
        var domainEvent = new TestDomainEvent();
        var after = DateTime.UtcNow;

        Assert.InRange(domainEvent.OccurredOn, before, after);
    }

    [Fact]
    public void OccurredOn_Kind_IsUtc()
    {
        var domainEvent = new TestDomainEvent();

        Assert.Equal(DateTimeKind.Utc, domainEvent.OccurredOn.Kind);
    }

    [Fact]
    public void OccurredOn_IsReadonly()
    {
        var domainEvent = new TestDomainEvent();
        var firstCall = domainEvent.OccurredOn;
        var secondCall = domainEvent.OccurredOn;

        Assert.Equal(firstCall, secondCall);
    }

    private sealed class TestDomainEvent : DomainEvent;
}
