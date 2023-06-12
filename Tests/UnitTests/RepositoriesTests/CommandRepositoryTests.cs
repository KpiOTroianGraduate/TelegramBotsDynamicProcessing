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

public class CommandRepositoryTests
{
    private readonly Mock<ITelegramContext> _contextMock;
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<CommandRepository>> _loggerMock;

    public CommandRepositoryTests()
    {
        _contextMock = new Mock<ITelegramContext>();
        _loggerMock = new Mock<ILogger<CommandRepository>>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task AddAsync_ShouldCallAdd()
    {
        // Arrange
        _contextMock.Setup(c => c.Commands).ReturnsDbSet(new List<Command>());
        var command = _fixture.Build<Command>().Without(c => c.TelegramBot).Without(c => c.CommandAction).Create();
        var commandRepository = new CommandRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        await commandRepository.AddAsync(command).ConfigureAwait(false);

        // Assert
        _contextMock.Verify(c => c.Commands.AddAsync(It.IsAny<Command>(), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task Get_ShouldReturnElement()
    {
        // Arrange
        var commands = _fixture.Build<Command>().Without(c => c.TelegramBot).Without(c => c.CommandAction).CreateMany(2)
            .ToList();
        _contextMock.Setup(c => c.Commands).ReturnsDbSet(commands);
        var commandRepository = new CommandRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var command = await commandRepository.GetAsync(commands.First().Id).ConfigureAwait(false);

        // Assert
        Assert.Equal(commands.First().Id, command.Id);
    }

    [Fact]
    public async Task GetGetFirstOrDefaultAsync_ShouldReturnNull()
    {
        // Arrange
        var commands = _fixture.Build<Command>().Without(c => c.TelegramBot).Without(c => c.CommandAction).CreateMany(2)
            .ToList();
        _contextMock.Setup(c => c.Commands).ReturnsDbSet(commands);
        var commandRepository = new CommandRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var command = await commandRepository.GetFirstOrDefaultAsync(c => c.Id == Guid.NewGuid()).ConfigureAwait(false);

        // Assert
        Assert.Null(command);
    }

    [Fact]
    public async Task GetGetFirstOrDefaultAsync_ShouldReturnElement()
    {
        // Arrange
        var commands = _fixture.Build<Command>().Without(c => c.TelegramBot).Without(c => c.CommandAction).CreateMany(2)
            .ToList();
        _contextMock.Setup(c => c.Commands).ReturnsDbSet(commands);
        var commandRepository = new CommandRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var command = await commandRepository.GetFirstOrDefaultAsync(c => c.Id == commands.First().Id)
            .ConfigureAwait(false);

        // Assert
        Assert.NotNull(command);
        Assert.Equal(commands.First().Id, command.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllElements()
    {
        // Arrange
        var commands = _fixture.Build<Command>().Without(c => c.TelegramBot).Without(c => c.CommandAction).CreateMany(2)
            .ToList();
        _contextMock.Setup(c => c.Commands).ReturnsDbSet(commands);
        var commandRepository = new CommandRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var result = await commandRepository.GetAllAsync().ConfigureAwait(false);

        // Assert=
        Assert.Equal(JsonConvert.SerializeObject(commands), JsonConvert.SerializeObject(result));
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDelete()
    {
        // Arrange
        var commands = _fixture.Build<Command>().Without(c => c.TelegramBot).Without(c => c.CommandAction).CreateMany(2)
            .ToList();
        _contextMock.Setup(c => c.Commands).ReturnsDbSet(commands);
        var commandRepository = new CommandRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        await commandRepository.DeleteAsync(commands.First().Id).ConfigureAwait(false);

        // Assert
        _contextMock.Verify(c => c.Commands.Remove(It.IsAny<Command>()));
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallUpdate()
    {
        // Arrange
        var commands = _fixture.Build<Command>().Without(c => c.TelegramBot).Without(c => c.CommandAction).CreateMany(2)
            .ToList();
        _contextMock.Setup(c => c.Commands).ReturnsDbSet(commands);
        var commandRepository = new CommandRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var result = await commandRepository.GetCommandsListAsync(c => c.Id == commands.First().Id)
            .ConfigureAwait(false);

        // Assert
        Assert.Equal(JsonConvert.SerializeObject(new List<Command> { commands.First() }),
            JsonConvert.SerializeObject(result));
    }
}