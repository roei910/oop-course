namespace FinanceGrid.Shared;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
