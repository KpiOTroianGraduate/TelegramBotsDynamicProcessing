using System.Security.Claims;
using AutoFixture;
using Contracts.Dto;
using Contracts.Dto.TelegramBot;
using Contracts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;
using UI.Controllers;
using Xunit;

namespace Tests.UnitTests.ControllersTests;

public class TelegramBotControllerTests
{
    private readonly ControllerContext _controllerContext;
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<TelegramBotController>> _loggerMock;
    private readonly Mock<ITelegramBotService> _telegramBotServiceMock;
    private readonly Mock<ITelegramService> _telegramServiceMock;
    private readonly Mock<IVerifyService> _verifyServiceMock;

    public TelegramBotControllerTests()
    {
        _telegramBotServiceMock = new Mock<ITelegramBotService>();
        _telegramServiceMock = new Mock<ITelegramService>();
        _verifyServiceMock = new Mock<IVerifyService>();
        _loggerMock = new Mock<ILogger<TelegramBotController>>();
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
    public async Task GetTelegramBotsAsync_ShouldReturnOk()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };

        // Act
        var result = await telegramBotController.GetTelegramBotsAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _telegramBotServiceMock.Verify(s => s.GetTelegramBotsAsync(It.IsAny<IEnumerable<Claim>>()));
        _telegramServiceMock.Verify(s => s.GetBotsInfoAsync(It.IsAny<List<TelegramBot>>()));
    }

    [Fact]
    public async Task GetTelegramBotsAsync_ShouldLogError_WhenThrows()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object);

        // Act
        var result = await telegramBotController.GetTelegramBotsAsync();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CreateTelegramBotAsync_ShouldReturnOk(bool isActive)
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _telegramServiceMock.Setup(s => s.IsBotAvailableAsync(It.IsAny<string>())).ReturnsAsync(true);
        var item = _fixture.Build<TelegramBotDto>().With(d => d.IsActive, isActive).Create();

        // Act
        var result = await telegramBotController.CreateTelegramBotAsync(item);

        // Assert
        Assert.IsType<OkResult>(result);
        _telegramServiceMock.Verify(s => s.IsBotAvailableAsync(It.IsAny<string>()));
        _telegramServiceMock.Verify(s => s.SetWebHookAsync(It.IsAny<string>()),
            isActive ? Times.Once() : Times.Never());
        _telegramBotServiceMock.Verify(s =>
            s.CreateTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<TelegramBotDto>()));
    }

    [Fact]
    public async Task CreateTelegramBotAsync_ShouldReturnNotFound_WhenTokenIsInvalid()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object);
        _telegramServiceMock.Setup(s => s.IsBotAvailableAsync(It.IsAny<string>())).ReturnsAsync(false);
        var item = _fixture.Create<TelegramBotDto>();

        // Act
        var result = await telegramBotController.CreateTelegramBotAsync(item);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _telegramServiceMock.Verify(s => s.IsBotAvailableAsync(It.IsAny<string>()));
    }

    [Fact]
    public async Task CreateTelegramBotAsync_ShouldLogError_WhenThrows()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object);
        _telegramServiceMock.Setup(s => s.IsBotAvailableAsync(It.IsAny<string>())).ThrowsAsync(new Exception());
        var item = _fixture.Create<TelegramBotDto>();

        // Act
        var result = await telegramBotController.CreateTelegramBotAsync(item);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateTelegramBotAsync_ShouldReturnOk(bool isActive)
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        _telegramServiceMock.Setup(s => s.IsBotAvailableAsync(It.IsAny<string>())).ReturnsAsync(true);
        var item = _fixture.Build<TelegramBotDto>().With(d => d.IsActive, isActive).Create();

        // Act
        var result = await telegramBotController.UpdateTelegramBotAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _telegramServiceMock.Verify(s => s.IsBotAvailableAsync(It.IsAny<string>()));
        _telegramServiceMock.Verify(s => s.SetWebHookAsync(It.IsAny<string>()),
            isActive ? Times.Once() : Times.Never());
        _telegramServiceMock.Verify(s => s.DeleteWebHookAsync(It.IsAny<string>()),
            !isActive ? Times.Once() : Times.Never());
        _telegramBotServiceMock.Verify(s => s.UpdateTelegramBotAsync(It.IsAny<Guid>(), It.IsAny<TelegramBotDto>()));
    }

    [Fact]
    public async Task UpdateTelegramBotAsync_ShouldReturnNotFound_WhenBotIsNotAvailable()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        var item = _fixture.Create<TelegramBotDto>();

        // Act
        var result = await telegramBotController.UpdateTelegramBotAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task UpdateTelegramBotAsync_ShouldReturnNotFound_WhenTokenIsInvalid()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        _telegramServiceMock.Setup(s => s.IsBotAvailableAsync(It.IsAny<string>())).ReturnsAsync(false);
        var item = _fixture.Create<TelegramBotDto>();

        // Act
        var result = await telegramBotController.UpdateTelegramBotAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task UpdateTelegramBotAsync_ShouldLogError_WhenThrows()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object);
        // Act
        var result = await telegramBotController.UpdateTelegramBotAsync(It.IsAny<Guid>(), It.IsAny<TelegramBotDto>());
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task DeleteTelegramBotAsync_ShouldReturnOk()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var item = _fixture.Create<TelegramBotDto>();
        _telegramBotServiceMock.Setup(s => s.GetTelegramBotAsync(It.IsAny<Guid>())).ReturnsAsync(item);

        // Act
        var result = await telegramBotController.DeleteTelegramBotAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _telegramBotServiceMock.Verify(s => s.GetTelegramBotAsync(It.IsAny<Guid>()));
        _telegramServiceMock.Verify(s => s.DeleteWebHookAsync(It.IsAny<string>()));
        _telegramBotServiceMock.Verify(s => s.DeleteTelegramBotAsync(It.IsAny<Guid>()));
    }

    [Fact]
    public async Task DeleteTelegramBotAsync_ShouldReturnNotFound_WhenTokenIsInvalid()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var result = await telegramBotController.DeleteTelegramBotAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task DeleteTelegramBotAsync_ShouldLogError_WhenThrows()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object);

        // Act
        var result = await telegramBotController.DeleteTelegramBotAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task GetTelegramBotDescription_ShouldReturnOk()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var item = _fixture.Create<TelegramBotDto>();
        _telegramBotServiceMock.Setup(s => s.GetTelegramBotAsync(It.IsAny<Guid>())).ReturnsAsync(item);

        // Act
        var result = await telegramBotController.GetDescriptionAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _telegramBotServiceMock.Verify(s => s.GetTelegramBotAsync(It.IsAny<Guid>()));
        _telegramServiceMock.Verify(s => s.GetDescriptionAsync(It.IsAny<string>()));
    }

    [Fact]
    public async Task GetTelegramBotDescription_ShouldReturnNotFound_WhenTokenIsInvalid()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var result = await telegramBotController.GetDescriptionAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task GetTelegramBotDescription_ShouldLogError_WhenThrows()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object);
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await telegramBotController.GetDescriptionAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task SetBotNameAsync_ShouldReturnOk()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var bot = _fixture.Create<TelegramBotDto>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var item = _fixture.Create<ValueDto>();
        _telegramBotServiceMock.Setup(s => s.GetTelegramBotAsync(It.IsAny<Guid>())).ReturnsAsync(bot);

        // Act
        var result = await telegramBotController.SetBotNameAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _telegramBotServiceMock.Verify(s => s.GetTelegramBotAsync(It.IsAny<Guid>()));
        _telegramServiceMock.Verify(s => s.ChangeNameAsync(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Fact]
    public async Task SetBotNameAsync_ShouldReturnNotFound_WhenTokenIsInvalid()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Create<ValueDto>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        // Act
        var result = await telegramBotController.SetBotNameAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task SetBotNameAsync_ShouldLogError_WhenThrows()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object);
        var item = _fixture.Create<ValueDto>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await telegramBotController.SetBotNameAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task SetDescriptionAsync_ShouldReturnOk()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var bot = _fixture.Create<TelegramBotDto>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var item = _fixture.Create<ValueDto>();
        _telegramBotServiceMock.Setup(s => s.GetTelegramBotAsync(It.IsAny<Guid>())).ReturnsAsync(bot);

        // Act
        var result = await telegramBotController.SetDescriptionAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _telegramBotServiceMock.Verify(s => s.GetTelegramBotAsync(It.IsAny<Guid>()));
        _telegramServiceMock.Verify(s => s.ChangeDescriptionAsync(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Fact]
    public async Task SetDescriptionAsync_ShouldReturnNotFound_WhenTokenIsInvalid()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Create<ValueDto>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        // Act
        var result = await telegramBotController.SetDescriptionAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task SetDescriptionAsync_ShouldLogError_WhenThrows()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object);
        var item = _fixture.Create<ValueDto>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await telegramBotController.SetDescriptionAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task SetShortDescriptionAsync_ShouldReturnOk()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var bot = _fixture.Create<TelegramBotDto>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var item = _fixture.Create<ValueDto>();
        _telegramBotServiceMock.Setup(s => s.GetTelegramBotAsync(It.IsAny<Guid>())).ReturnsAsync(bot);

        // Act
        var result = await telegramBotController.SetShortDescriptionAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _telegramBotServiceMock.Verify(s => s.GetTelegramBotAsync(It.IsAny<Guid>()));
        _telegramServiceMock.Verify(s => s.ChangeShortDescriptionAsync(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Fact]
    public async Task SetShortDescriptionAsync_ShouldReturnNotFound_WhenTokenIsInvalid()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Create<ValueDto>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        // Act
        var result = await telegramBotController.SetShortDescriptionAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task SetShortDescriptionAsync_ShouldLogError_WhenThrows()
    {
        // Arrange
        var telegramBotController = new TelegramBotController(_telegramBotServiceMock.Object,
            _telegramServiceMock.Object, _verifyServiceMock.Object, _loggerMock.Object);
        var item = _fixture.Create<ValueDto>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await telegramBotController.SetShortDescriptionAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }
}