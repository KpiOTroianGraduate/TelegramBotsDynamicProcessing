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

public class TelegramBotRepositoryTests
{
    private readonly Mock<ITelegramContext> _contextMock;
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<TelegramBotRepository>> _loggerMock;

    public TelegramBotRepositoryTests()
    {
        _contextMock = new Mock<ITelegramContext>();
        _loggerMock = new Mock<ILogger<TelegramBotRepository>>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task AddAsync_ShouldCallAdd()
    {
        // Arrange
        _contextMock.Setup(c => c.TelegramBots).ReturnsDbSet(new List<TelegramBot>());
        var telegramBot = _fixture.Build<TelegramBot>().Without(b => b.User).Without(b => b.Commands)
            .Without(b => b.CommandActions).Create();
        var telegramBotRepository = new TelegramBotRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        await telegramBotRepository.AddAsync(telegramBot).ConfigureAwait(false);

        // Assert
        _contextMock.Verify(c => c.TelegramBots.AddAsync(It.IsAny<TelegramBot>(), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task Get_ShouldReturnElement()
    {
        // Arrange
        var telegramBots = _fixture.Build<TelegramBot>().Without(b => b.User).Without(b => b.Commands)
            .Without(b => b.CommandActions).CreateMany(2).ToList();
        _contextMock.Setup(c => c.TelegramBots).ReturnsDbSet(telegramBots);
        var telegramBotRepository = new TelegramBotRepository(_contextMock.Object, _loggerMock.Object);
        // Act
        var telegramBot = await telegramBotRepository.GetAsync(telegramBots.First().Id).ConfigureAwait(false);
        // Assert
        Assert.Equal(telegramBots.First().Id, telegramBot.Id);
    }

    [Fact]
    public async Task Get_ShouldReturnElementWithCommands()
    {
        // Arrange
        var telegramBots = _fixture.Build<TelegramBot>().Without(b => b.User).Without(b => b.Commands)
            .Without(b => b.CommandActions).CreateMany(2).ToList();
        _contextMock.Setup(c => c.TelegramBots).ReturnsDbSet(telegramBots);
        var telegramBotRepository = new TelegramBotRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var telegramBot = await telegramBotRepository.GetAsync(telegramBots.First().Id).ConfigureAwait(false);

        // Assert
        Assert.Equal(telegramBots.First().Id, telegramBot.Id);
    }

    [Fact]
    public async Task GetGetFirstOrDefaultAsync_ShouldReturnNull()
    {
        // Arrange
        var telegramBots = _fixture.Build<TelegramBot>().Without(b => b.User).Without(b => b.Commands)
            .Without(b => b.CommandActions).CreateMany(2).ToList();
        _contextMock.Setup(c => c.TelegramBots).ReturnsDbSet(telegramBots);
        var telegramBotRepository = new TelegramBotRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var telegramBot = await telegramBotRepository.GetFirstOrDefaultAsync(c => c.Id == Guid.NewGuid())
            .ConfigureAwait(false);

        // Assert
        Assert.Null(telegramBot);
    }

    [Fact]
    public async Task GetGetFirstOrDefaultAsync_ShouldReturnElement()
    {
        // Arrange
        var telegramBots = _fixture.Build<TelegramBot>().Without(b => b.User).Without(b => b.Commands)
            .Without(b => b.CommandActions).CreateMany(2).ToList();
        _contextMock.Setup(c => c.TelegramBots).ReturnsDbSet(telegramBots);
        var telegramBotRepository = new TelegramBotRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var telegramBot = await telegramBotRepository.GetFirstOrDefaultAsync(c => c.Id == telegramBots.First().Id)
            .ConfigureAwait(false);

        // Assert
        Assert.NotNull(telegramBot);
        Assert.Equal(JsonConvert.SerializeObject(telegramBots.First()), JsonConvert.SerializeObject(telegramBot));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllElements()
    {
        // Arrange
        var telegramBots = _fixture.Build<TelegramBot>().Without(b => b.User).Without(b => b.Commands)
            .Without(b => b.CommandActions).CreateMany(2).ToList();
        _contextMock.Setup(c => c.TelegramBots).ReturnsDbSet(telegramBots);
        var telegramBotRepository = new TelegramBotRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var result = await telegramBotRepository.GetAllAsync().ConfigureAwait(false);

        // Assert
        Assert.Equal(JsonConvert.SerializeObject(telegramBots), JsonConvert.SerializeObject(result));
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDelete()
    {
        // Arrange
        var telegramBots = _fixture.Build<TelegramBot>().Without(b => b.User).Without(b => b.Commands)
            .Without(b => b.CommandActions).CreateMany(2).ToList();
        _contextMock.Setup(c => c.TelegramBots).ReturnsDbSet(telegramBots);
        var telegramBotRepository = new TelegramBotRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        await telegramBotRepository.DeleteAsync(telegramBots.First().Id).ConfigureAwait(false);

        // Assert
        _contextMock.Verify(c => c.TelegramBots.Remove(It.IsAny<TelegramBot>()));
    }

    [Fact]
    public async Task GetFirstOrDefaultAsync_ShouldReturnElement()
    {
        // Arrange
        var telegramBots = _fixture.Build<TelegramBot>().Without(b => b.User).Without(b => b.Commands)
            .Without(b => b.CommandActions).CreateMany(2).ToList();
        _contextMock.Setup(c => c.TelegramBots).ReturnsDbSet(telegramBots);
        var telegramBotRepository = new TelegramBotRepository(_contextMock.Object, _loggerMock.Object);
        // Act
        var telegramBot = await telegramBotRepository.GetFirstOrDefaultAsync(c => c.Id == telegramBots.First().Id)
            .ConfigureAwait(false);
        // Assert
        Assert.NotNull(telegramBot);
        Assert.Equal(JsonConvert.SerializeObject(telegramBots.First()), JsonConvert.SerializeObject(telegramBot));
    }

    [Fact]
    public async Task GetTelegramBotListAsync_ShouldReturnList()
    {
        // Arrange
        var telegramBots = _fixture.Build<TelegramBot>().Without(b => b.User).Without(b => b.Commands)
            .Without(b => b.CommandActions).CreateMany(2).ToList();
        _contextMock.Setup(c => c.TelegramBots).ReturnsDbSet(telegramBots);
        var telegramBotRepository = new TelegramBotRepository(_contextMock.Object, _loggerMock.Object);

        // Act
        var result = await telegramBotRepository.GetTelegramBotListAsync(c => c.Id == telegramBots.First().Id)
            .ConfigureAwait(false);

        // Assert
        Assert.Equal(JsonConvert.SerializeObject(new List<TelegramBot> { telegramBots.First() }),
            JsonConvert.SerializeObject(result));
    }
}