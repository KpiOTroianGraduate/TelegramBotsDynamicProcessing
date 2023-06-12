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

public class CommandActionRepositoryTests
{
    private readonly Mock<ITelegramContext> _contextMock;
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<CommandActionRepository>> _loggerMock;

    public CommandActionRepositoryTests()
    {
        _contextMock = new Mock<ITelegramContext>();
        _loggerMock = new Mock<ILogger<CommandActionRepository>>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task AddAsync_ShouldCallAdd()
    {
        // Arrange
        _contextMock.Setup(c => c.CommandActions).ReturnsDbSet(new List<CommandAction>());
        var commandAction = _fixture.Build<CommandAction>().Without(a => a.Commands).Without(a => a.TelegramBot)
            .Create();
        var commandActionRepository = new CommandActionRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        await commandActionRepository.AddAsync(commandAction).ConfigureAwait(false);

        // Assert
        _contextMock.Verify(c => c.CommandActions.AddAsync(It.IsAny<CommandAction>(), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task Get_ShouldReturnElement()
    {
        // Arrange
        var commandActions = _fixture.Build<CommandAction>().Without(a => a.Commands).Without(a => a.TelegramBot)
            .CreateMany(2).ToList();
        _contextMock.Setup(c => c.CommandActions).ReturnsDbSet(commandActions);
        var commandActionRepository = new CommandActionRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var commandAction = await commandActionRepository.GetAsync(commandActions.First().Id).ConfigureAwait(false);

        // Assert
        Assert.Equal(commandActions.First().Id, commandAction.Id);
    }

    [Fact]
    public async Task GetGetFirstOrDefaultAsync_ShouldReturnNull()
    {
        // Arrange
        var commandActions = _fixture.Build<CommandAction>().Without(a => a.Commands).Without(a => a.TelegramBot)
            .CreateMany(2).ToList();
        _contextMock.Setup(c => c.CommandActions).ReturnsDbSet(commandActions);
        var commandActionRepository = new CommandActionRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var commandAction = await commandActionRepository.GetFirstOrDefaultAsync(a => a.Id == Guid.NewGuid())
            .ConfigureAwait(false);

        // Assert
        Assert.Null(commandAction);
    }

    [Fact]
    public async Task GetGetFirstOrDefaultAsync_ShouldReturnNotNull()
    {
        // Arrange
        var commandActions = _fixture.Build<CommandAction>().Without(a => a.Commands).Without(a => a.TelegramBot)
            .CreateMany(2).ToList();
        _contextMock.Setup(c => c.CommandActions).ReturnsDbSet(commandActions);
        var commandActionRepository = new CommandActionRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var commandAction = await commandActionRepository.GetFirstOrDefaultAsync(a => a.Id == commandActions.First().Id)
            .ConfigureAwait(false);

        // Assert
        Assert.NotNull(commandAction);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllElements()
    {
        // Arrange
        var commandActions = _fixture.Build<CommandAction>().Without(a => a.Commands).Without(a => a.TelegramBot)
            .CreateMany(2).ToList();
        _contextMock.Setup(c => c.CommandActions).ReturnsDbSet(commandActions);
        var commandActionRepository = new CommandActionRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var result = await commandActionRepository.GetAllAsync().ConfigureAwait(false);

        // Assert
        Assert.Equal(JsonConvert.SerializeObject(commandActions), JsonConvert.SerializeObject(result));
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDelete()
    {
        // Arrange
        var commandActions = _fixture.Build<CommandAction>().Without(a => a.Commands).Without(a => a.TelegramBot)
            .CreateMany(2).ToList();
        _contextMock.Setup(c => c.CommandActions).ReturnsDbSet(commandActions);
        var commandActionRepository = new CommandActionRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        await commandActionRepository.DeleteAsync(commandActions.First().Id).ConfigureAwait(false);

        // Assert
        _contextMock.Verify(c => c.CommandActions.Remove(It.IsAny<CommandAction>()));
    }

    [Fact]
    public async Task GetCommandActionListAsync_ShouldReturnElements()
    {
        // Arrange
        var commandActions = _fixture.Build<CommandAction>().Without(a => a.Commands).Without(a => a.TelegramBot)
            .CreateMany(2).ToList();
        _contextMock.Setup(c => c.CommandActions).ReturnsDbSet(commandActions);
        var commandActionRepository = new CommandActionRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var commandAction = await commandActionRepository
            .GetCommandActionListAsync(a => a.Id == commandActions.First().Id)
            .ConfigureAwait(false);

        // Assert
        Assert.Equal(JsonConvert.SerializeObject(new List<CommandAction> { commandAction.First() }),
            JsonConvert.SerializeObject(commandAction));
    }
}