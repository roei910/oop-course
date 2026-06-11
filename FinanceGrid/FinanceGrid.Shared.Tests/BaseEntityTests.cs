namespace FinanceGrid.Shared.Tests;

public class BaseEntityTests
{
    private sealed class TestEntity : BaseEntity<Guid>
    {
        public TestEntity(Guid id) => Id = id;
    }

    private sealed class OtherTestEntity : BaseEntity<Guid>
    {
        public OtherTestEntity(Guid id) => Id = id;
    }

    private sealed class IntTestEntity : BaseEntity<int>
    {
        public IntTestEntity(int id) => Id = id;
    }

    [Fact]
    public void Equals_SameInstance_ReturnsTrue()
    {
        var entity = new TestEntity(Guid.NewGuid());
        Assert.True(entity.Equals(entity));
    }

    [Fact]
    public void Equals_SameIdAndType_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        Assert.True(entity1.Equals(entity2));
    }

    [Fact]
    public void Equals_DifferentId_ReturnsFalse()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        Assert.False(entity1.Equals(entity2));
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        var entity = new TestEntity(Guid.NewGuid());
        Assert.False(entity.Equals(null));
    }

    [Fact]
    public void Equals_DifferentConcreteType_SameIdType_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new OtherTestEntity(id);

        Assert.True(entity1.Equals(entity2));
    }

    [Fact]
    public void Equals_DifferentGenericType_ReturnsFalse()
    {
        var guidEntity = new TestEntity(Guid.NewGuid());
        object intEntity = new IntTestEntity(42);

        Assert.False(guidEntity.Equals(intEntity));
    }

    [Fact]
    public void GetHashCode_SameId_SameHash()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentId_DifferentHash()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        Assert.NotEqual(entity1.GetHashCode(), entity2.GetHashCode());
    }

    [Fact]
    public void EqualityOperator_BothNull_ReturnsTrue()
    {
        TestEntity? left = null;
        TestEntity? right = null;

        Assert.True(left == right);
    }

    [Fact]
    public void EqualityOperator_LeftNull_ReturnsFalse()
    {
        TestEntity? left = null;
        var right = new TestEntity(Guid.NewGuid());

        Assert.False(left == right);
    }

    [Fact]
    public void EqualityOperator_RightNull_ReturnsFalse()
    {
        var left = new TestEntity(Guid.NewGuid());
        TestEntity? right = null;

        Assert.False(left == right);
    }

    [Fact]
    public void EqualityOperator_SameId_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var left = new TestEntity(id);
        var right = new TestEntity(id);

        Assert.True(left == right);
    }

    [Fact]
    public void InequalityOperator_SameId_ReturnsFalse()
    {
        var id = Guid.NewGuid();
        var left = new TestEntity(id);
        var right = new TestEntity(id);

        Assert.False(left != right);
    }

    [Fact]
    public void InequalityOperator_DifferentId_ReturnsTrue()
    {
        var left = new TestEntity(Guid.NewGuid());
        var right = new TestEntity(Guid.NewGuid());

        Assert.True(left != right);
    }

    [Fact]
    public void AddDomainEvent_AddsToCollection()
    {
        var entity = new TestEntity(Guid.NewGuid());
        var domainEvent = new TestDomainEvent();

        entity.GetType().GetMethod("AddDomainEvent",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(entity, [domainEvent]);

        Assert.Contains(domainEvent, entity.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_RemovesAllEvents()
    {
        var entity = new TestEntity(Guid.NewGuid());
        var domainEvent = new TestDomainEvent();

        entity.GetType().GetMethod("AddDomainEvent",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(entity, [domainEvent]);

        entity.ClearDomainEvents();

        Assert.Empty(entity.DomainEvents);
    }

    [Fact]
    public void DomainEvents_InitiallyEmpty()
    {
        var entity = new TestEntity(Guid.NewGuid());
        Assert.Empty(entity.DomainEvents);
    }

    private sealed class TestDomainEvent : DomainEvent;
}
