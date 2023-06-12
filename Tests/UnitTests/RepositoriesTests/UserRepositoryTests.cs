using AutoFixture;
using Contracts.Entities;
using DAL;
using DAL.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Tests.UnitTests.RepositoriesTests;

public class UserRepositoryTests
{
    private readonly Mock<ITelegramContext> _contextMock;
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<UserRepository>> _loggerMock;

    public UserRepositoryTests()
    {
        _contextMock = new Mock<ITelegramContext>();
        _loggerMock = new Mock<ILogger<UserRepository>>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task AddAsync_ShouldCallAdd()
    {
        // Arrange
        _contextMock.Setup(c => c.Users).ReturnsDbSet(new List<User>());
        var user = _fixture.Build<User>().Without(u => u.TelegramBots).Create();
        var userRepository = new UserRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        await userRepository.AddAsync(user).ConfigureAwait(false);

        // Assert
        _contextMock.Verify(c => c.Users.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task Get_ShouldReturnElement()
    {
        // Arrange
        var users = _fixture.Build<User>().Without(u => u.TelegramBots).CreateMany(2).ToList();
        _contextMock.Setup(c => c.Users).ReturnsDbSet(users);
        var userRepository = new UserRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var user = await userRepository.GetAsync(users.First().Id).ConfigureAwait(false);

        // Assert
        Assert.Equal(users.First().Id, user.Id);
    }

    [Fact]
    public async Task GetGetFirstOrDefaultAsync_ShouldReturnNull()
    {
        // Arrange
        var users = _fixture.Build<User>().Without(u => u.TelegramBots).CreateMany(2).ToList();
        _contextMock.Setup(c => c.Users).ReturnsDbSet(users);
        var userRepository = new UserRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var user = await userRepository.GetFirstOrDefaultAsync(u => u.Id == Guid.NewGuid()).ConfigureAwait(false);

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task GetGetFirstOrDefaultAsync_ShouldReturnNotNull()
    {
        // Arrange
        var users = _fixture.Build<User>().Without(u => u.TelegramBots).CreateMany(2).ToList();
        _contextMock.Setup(c => c.Users).ReturnsDbSet(users);
        var userRepository = new UserRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var user = await userRepository.GetFirstOrDefaultAsync(u => u.Id == users.First().Id).ConfigureAwait(false);

        // Assert
        Assert.NotNull(user);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllElements()
    {
        // Arrange
        var users = _fixture.Build<User>().Without(u => u.TelegramBots).CreateMany(2).ToList();
        _contextMock.Setup(c => c.Users).ReturnsDbSet(users);
        var userRepository = new UserRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var result = await userRepository.GetAllAsync().ConfigureAwait(false);

        // Assert
        Assert.Equal(JsonConvert.SerializeObject(users), JsonConvert.SerializeObject(result));
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDelete()
    {
        // Arrange
        var users = _fixture.Build<User>().Without(u => u.TelegramBots).CreateMany(2).ToList();
        _contextMock.Setup(c => c.Users).ReturnsDbSet(users);
        var userRepository = new UserRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        await userRepository.DeleteAsync(users.First().Id).ConfigureAwait(false);

        // Assert
        _contextMock.Verify(c => c.Users.Remove(It.IsAny<User>()));
    }
}