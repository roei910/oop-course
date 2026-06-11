namespace FinanceGrid.Shared.Tests;

public class ConfigurationKeysTests
{
    [Fact]
    public void YahooFinanceKeys_AreSet()
    {
        Assert.Equal("YahooFinance127", ConfigurationKeys.YahooFinance127);
        Assert.Equal("YahooFinance15", ConfigurationKeys.YahooFinance15);
        Assert.Equal("YahooFinance1", ConfigurationKeys.YahooFinance1);
    }

    [Fact]
    public void RealTimeFinanceData_IsSet()
    {
        Assert.Equal("RealTimeFinanceData", ConfigurationKeys.RealTimeFinanceData);
    }
}
