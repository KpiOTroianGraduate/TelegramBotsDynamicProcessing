using System.Security.Claims;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;
using Telegram.Bot.Types;
using UI.Controllers;
using Xunit;

namespace Tests.UnitTests.ControllersTests;

public class TelegramControllerTests
{
    private readonly ControllerContext _controllerContext;
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<TelegramController>> _loggerMock;
    private readonly Mock<ITelegramService> _telegramServiceMock;

    public TelegramControllerTests()
    {
        _telegramServiceMock = new Mock<ITelegramService>();
        _loggerMock = new Mock<ILogger<TelegramController>>();
        _controllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
            }
        };
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetUpdateFromTelegram_ShouldLogError()
    {
        // Arrange
        var controller = new TelegramController(_telegramServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _telegramServiceMock.Setup(s => s.ProcessMessageAsync(It.IsAny<string>(), It.IsAny<Update>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await controller.GetUpdateFromTelegram(_fixture.Create<string>(), It.IsAny<Update>());

        // Assert
        Assert.IsType<BadRequestResult>(result);
        _telegramServiceMock.Verify(s => s.ProcessMessageAsync(It.IsAny<string>(), It.IsAny<Update>()));
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task GetUpdateFromTelegram_ShouldReturnOk()
    {
        // Arrange
        var controller = new TelegramController(_telegramServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };

        // Act
        var result = await controller.GetUpdateFromTelegram(_fixture.Create<string>(), It.IsAny<Update>());

        // Assert
        Assert.IsType<OkResult>(result);
        _telegramServiceMock.Verify(s => s.ProcessMessageAsync(It.IsAny<string>(), It.IsAny<Update>()));
    }
}