namespace FinanceGrid.Shared.Tests;

public class AppVariablesTests
{
    [Fact]
    public void MaxAllowedAnalysisPerDay_Is10()
    {
        Assert.Equal(10, AppVariables.MAX_ALLOWED_ANALYSIS_PER_DAY);
    }

    [Fact]
    public void MinuteIntervalBetweenUpdate_Is60()
    {
        Assert.Equal(60, AppVariables.MINUTE_INTERVAL_BETWEEN_UPDATE);
    }

    [Fact]
    public void MarketHours_AreCorrect()
    {
        Assert.Equal(1, AppVariables.OPEN_MARKET_DAY);
        Assert.Equal(5, AppVariables.CLOSED_MARKET_DAY);
        Assert.Equal(13, AppVariables.OPEN_MARKET_HOURS);
        Assert.Equal(19, AppVariables.CLOSED_MARKET_HOURS);
        Assert.Equal(30, AppVariables.OPEN_MARKET_MINUTES);
    }

    [Fact]
    public void PasswordSalt_IsSet()
    {
        Assert.NotNull(AppVariables.PASSWORD_SALT);
        Assert.NotEmpty(AppVariables.PASSWORD_SALT);
    }
}
