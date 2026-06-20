using FinanceGrid.Users.Application.Services;
using FinanceGrid.Users.Domain.Entities;
using FinanceGrid.Users.Domain.Interfaces;

namespace FinanceGrid.Users.Tests;

public class ShareServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IUserStockWatchRepository> _watchRepoMock = new();
    private readonly ShareService _service;

    public ShareServiceTests()
    {
        _service = new ShareService(_userRepoMock.Object, _watchRepoMock.Object);
    }

    [Fact]
    public async Task AddShareAsync_UserNotFound_ReturnsNull()
    {
        _userRepoMock.Setup(r => r.GetAsync("unknown@test.com")).ReturnsAsync((User?)null);

        var result = await _service.AddShareAsync("unknown@test.com", "Tech", "AAPL", 150, DateTime.UtcNow, 10);

        Assert.Null(result);
        _watchRepoMock.Verify(r => r.UpsertWatchAndAddShareAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<Share>()), Times.Never);
    }

    [Fact]
    public async Task AddShareAsync_CreatesShareWithGuidAndUpserts()
    {
        var user = new User { Email = "test@test.com", WatchListNames = ["Tech"] };
        _userRepoMock.Setup(r => r.GetAsync("test@test.com")).ReturnsAsync(user);

        var result = await _service.AddShareAsync("test@test.com", "Tech", "aapl", 150, DateTime.UtcNow, 10);

        Assert.NotNull(result);
        Assert.Equal(150, result.PurchasingPrice);
        Assert.Equal(10, result.Amount);
        Assert.NotNull(result.Id);
        _watchRepoMock.Verify(r => r.UpsertWatchAndAddShareAsync(
            "test@test.com", "Tech", "AAPL", result.Id, result), Times.Once);
    }

    [Fact]
    public async Task AddShareAsync_AddsListNameToWatchListNames()
    {
        var user = new User { Email = "test@test.com", WatchListNames = [] };
        _userRepoMock.Setup(r => r.GetAsync("test@test.com")).ReturnsAsync(user);

        await _service.AddShareAsync("test@test.com", "NewList", "aapl", 150, DateTime.UtcNow, 10);

        Assert.Contains("NewList", user.WatchListNames);
    }

    [Fact]
    public async Task AddShareAsync_DoesNotDuplicateListName()
    {
        var user = new User { Email = "test@test.com", WatchListNames = ["ExistingList"] };
        _userRepoMock.Setup(r => r.GetAsync("test@test.com")).ReturnsAsync(user);

        await _service.AddShareAsync("test@test.com", "ExistingList", "aapl", 150, DateTime.UtcNow, 10);

        Assert.Single(user.WatchListNames);
    }

    [Fact]
    public async Task RemoveShareAsync_DelegatesToWatchRepo()
    {
        await _service.RemoveShareAsync("test@test.com", "Tech", "aapl", "share1");

        _watchRepoMock.Verify(r => r.RemoveShareFromWatchAsync(
            "test@test.com", "Tech", "AAPL", "share1"), Times.Once);
    }

    [Fact]
    public async Task AddUserListAsync_UserNotFound_DoesNothing()
    {
        _userRepoMock.Setup(r => r.GetAsync("unknown@test.com")).ReturnsAsync((User?)null);

        await _service.AddUserListAsync("unknown@test.com", "NewList");
    }

    [Fact]
    public async Task AddUserListAsync_AddsListToUser()
    {
        var user = new User { Email = "test@test.com", WatchListNames = [] };
        _userRepoMock.Setup(r => r.GetAsync("test@test.com")).ReturnsAsync(user);

        await _service.AddUserListAsync("test@test.com", "NewList");

        Assert.Contains("NewList", user.WatchListNames);
    }

    [Fact]
    public async Task RemoveUserListAsync_DeletesWatchesAndRemovesList()
    {
        var user = new User { Email = "test@test.com", WatchListNames = ["OldList"] };
        _userRepoMock.Setup(r => r.GetAsync("test@test.com")).ReturnsAsync(user);

        await _service.RemoveUserListAsync("test@test.com", "OldList");

        _watchRepoMock.Verify(r => r.DeleteWatchesByListAsync("test@test.com", "OldList"), Times.Once);
        Assert.DoesNotContain("OldList", user.WatchListNames);
    }

    [Fact]
    public async Task AddWatchingStockAsync_CreatesWatchWithUppercasedSymbol()
    {
        await _service.AddWatchingStockAsync("test@test.com", "Tech", "aapl");

        _watchRepoMock.Verify(r => r.InsertWatchAsync(
            It.Is<UserStockWatch>(w =>
                w.UserEmail == "test@test.com" && w.StockSymbol == "AAPL" && w.ListName == "Tech")),
            Times.Once);
    }

    [Fact]
    public async Task RemoveWatchingStockAsync_DelegatesWithUppercasedSymbol()
    {
        await _service.RemoveWatchingStockAsync("test@test.com", "Tech", "aapl");

        _watchRepoMock.Verify(r => r.DeleteWatchAsync("test@test.com", "Tech", "AAPL"), Times.Once);
    }

    [Fact]
    public async Task UpdateWatchingStockNoteAsync_DelegatesWithUppercasedSymbol()
    {
        await _service.UpdateWatchingStockNoteAsync("test@test.com", "Tech", "aapl", "my note");

        _watchRepoMock.Verify(r => r.UpdateWatchNoteAsync(
            "test@test.com", "Tech", "AAPL", "my note"), Times.Once);
    }
}
