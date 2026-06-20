using FinanceGrid.FinancialData.Domain.Entities;

namespace FinanceGrid.FinancialData.Domain.Interfaces;

public interface IStockMarketTime
{
    bool IsMarketOpen(DateTime date);
    bool ShouldStockBeUpdated(Stock stock);
    DateTime LastMarketCloseDateTime();
}
