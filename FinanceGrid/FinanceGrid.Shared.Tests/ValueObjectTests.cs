namespace FinanceGrid.Shared.Tests;

public class ValueObjectTests
{
    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var vo1 = new TestValueObject("abc", 42);
        var vo2 = new TestValueObject("abc", 42);

        Assert.True(vo1.Equals(vo2));
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var vo1 = new TestValueObject("abc", 42);
        var vo2 = new TestValueObject("xyz", 99);

        Assert.False(vo1.Equals(vo2));
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        var vo = new TestValueObject("abc", 42);

        Assert.False(vo.Equals(null));
    }

    [Fact]
    public void Equals_SameInstance_ReturnsTrue()
    {
        var vo = new TestValueObject("abc", 42);

        Assert.True(vo.Equals(vo));
    }

    [Fact]
    public void GetHashCode_SameValues_SameHash()
    {
        var vo1 = new TestValueObject("abc", 42);
        var vo2 = new TestValueObject("abc", 42);

        Assert.Equal(vo1.GetHashCode(), vo2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentValues_DifferentHash()
    {
        var vo1 = new TestValueObject("abc", 42);
        var vo2 = new TestValueObject("xyz", 99);

        Assert.NotEqual(vo1.GetHashCode(), vo2.GetHashCode());
    }

    [Fact]
    public void EqualityOperator_SameValues_ReturnsTrue()
    {
        var left = new TestValueObject("abc", 42);
        var right = new TestValueObject("abc", 42);

        Assert.True(left == right);
    }

    [Fact]
    public void EqualityOperator_DifferentValues_ReturnsFalse()
    {
        var left = new TestValueObject("abc", 42);
        var right = new TestValueObject("xyz", 99);

        Assert.False(left == right);
    }

    [Fact]
    public void InequalityOperator_SameValues_ReturnsFalse()
    {
        var left = new TestValueObject("abc", 42);
        var right = new TestValueObject("abc", 42);

        Assert.False(left != right);
    }

    [Fact]
    public void InequalityOperator_DifferentValues_ReturnsTrue()
    {
        var left = new TestValueObject("abc", 42);
        var right = new TestValueObject("xyz", 99);

        Assert.True(left != right);
    }

    [Fact]
    public void Equals_WithSingleComponent_Works()
    {
        var vo1 = new SingleComponentValueObject(5);
        var vo2 = new SingleComponentValueObject(5);
        var vo3 = new SingleComponentValueObject(10);

        Assert.True(vo1.Equals(vo2));
        Assert.False(vo1.Equals(vo3));
    }

    [Fact]
    public void Equals_WithThreeComponents_Works()
    {
        var vo1 = new ThreeComponentValueObject(1, "a", true);
        var vo2 = new ThreeComponentValueObject(1, "a", true);
        var vo3 = new ThreeComponentValueObject(1, "a", false);
        var vo4 = new ThreeComponentValueObject(2, "b", false);

        Assert.True(vo1 == vo2);
        Assert.False(vo1 == vo3);
        Assert.False(vo1 == vo4);
    }

    [Fact]
    public void GetHashCode_ZeroComponents_Throws()
    {
        var vo = new ZeroComponentValueObject();
        Assert.Throws<InvalidOperationException>(() => vo.GetHashCode());
    }

    private sealed class TestValueObject : ValueObject
    {
        private readonly string _name;
        private readonly int _value;

        public TestValueObject(string name, int value)
        {
            _name = name;
            _value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _name;
            yield return _value;
        }
    }

    private sealed class SingleComponentValueObject : ValueObject
    {
        private readonly int _value;

        public SingleComponentValueObject(int value) => _value = value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value;
        }
    }

    private sealed class ThreeComponentValueObject : ValueObject
    {
        private readonly int _a;
        private readonly string _b;
        private readonly bool _c;

        public ThreeComponentValueObject(int a, string b, bool c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _a;
            yield return _b;
            yield return _c;
        }
    }

    private sealed class ZeroComponentValueObject : ValueObject
    {
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }
    }
}
