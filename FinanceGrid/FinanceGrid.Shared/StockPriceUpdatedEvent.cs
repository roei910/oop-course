namespace FinanceGrid.Shared;

public class StockPriceUpdatedEvent : DomainEvent
{
    public string Symbol { get; }
    public decimal OldPrice { get; }
    public decimal NewPrice { get; }
    public decimal ChangePercent { get; }
    public DateTime UpdatedAt { get; }

    public StockPriceUpdatedEvent(string symbol, decimal oldPrice, decimal newPrice, decimal changePercent)
    {
        Symbol = symbol;
        OldPrice = oldPrice;
        NewPrice = newPrice;
        ChangePercent = changePercent;
        UpdatedAt = DateTime.UtcNow;
    }
}
