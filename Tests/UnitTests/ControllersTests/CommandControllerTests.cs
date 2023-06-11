using System.Security.Claims;
using AutoFixture;
using Contracts.Dto.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;
using UI.Controllers;
using Xunit;

namespace Tests.UnitTests.ControllersTests;

public class CommandControllerTests
{
    private readonly Mock<ICommandService> _commandServiceMock;
    private readonly ControllerContext _controllerContext;
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<CommandController>> _loggerMock;
    private readonly Mock<IVerifyService> _verifyServiceMock;

    public CommandControllerTests()
    {
        _commandServiceMock = new Mock<ICommandService>();
        _verifyServiceMock = new Mock<IVerifyService>();
        _loggerMock = new Mock<ILogger<CommandController>>();
        _controllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
            }
        };
        _fixture = new Fixture();
    }


    public static IEnumerable<object[]> Guids
    {
        get
        {
            yield return new object[] { null! };
            yield return new object[] { Guid.NewGuid() };
        }
    }

    [Fact]
    public async Task GetCommandsByTelegramBotIdAsync_WhenTelegramBotIdIsNotValid_ReturnsNotFound()
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };

        // Act
        var result = await controller.GetCommandsByTelegramBotIdAsync(It.IsAny<Guid>()).ConfigureAwait(false);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task GetCommandsByTelegramBotIdAsync_ShouldReturnOk()
    {
        // Arrange
        var telegramBotId = _fixture.Create<Guid>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };

        // Act
        var result = await controller.GetCommandsByTelegramBotIdAsync(telegramBotId).ConfigureAwait(false);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _commandServiceMock.Verify(s => s.GetCommandsByTelegramBotIdAsync(It.IsAny<Guid>()));
    }

    [Fact]
    public async Task GetCommandsByTelegramBotIdAsync_ShouldLogError()
    {
        // Arrange
        var telegramBotId = _fixture.Create<Guid>();
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };

        // Act
        var result = await controller.GetCommandsByTelegramBotIdAsync(telegramBotId).ConfigureAwait(false);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task CreateCommandAsync_WhenTelegramBotIdIsNotValid_ReturnsNotFound()
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Create<CommandDto>();

        // Act
        var result = await controller.CreateCommandAsync(item).ConfigureAwait(false);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Theory]
    [MemberData(nameof(Guids))]
    public async Task CreateCommandAsync_ShouldReturnOk(Guid? commandActionId)
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Build<CommandDto>().With(c => c.CommandActionId, commandActionId).Create();

        // Act
        var result = await controller.CreateCommandAsync(item).ConfigureAwait(false);

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _verifyServiceMock.Verify(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()),
            commandActionId == null ? Times.Never() : Times.Once());
        _commandServiceMock.Verify(s => s.CreateCommandAsync(It.IsAny<CommandDto>()));
    }

    [Fact]
    public async Task CreateCommandAsync_WhenCommandActionIdIsNotValid_ReturnsNotFound()
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Create<CommandDto>();

        // Act
        var result = await controller.CreateCommandAsync(item).ConfigureAwait(false);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _verifyServiceMock.Verify(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task CreateCommandAsync_ShouldLogError()
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Create<CommandDto>();

        // Act
        var result = await controller.CreateCommandAsync(item).ConfigureAwait(false);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task UpdateCommandAsync_WhenCommandIdIsNotValid_ReturnsNotFound()
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Create<CommandDto>();

        // Act
        var result = await controller.UpdateCommandAsync(It.IsAny<Guid>(), item).ConfigureAwait(false);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Theory]
    [MemberData(nameof(Guids))]
    public async Task UpdateCommandAsync_ShouldReturnOk(Guid? commandActionId)
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Build<CommandDto>().With(c => c.CommandActionId, commandActionId).Create();

        // Act
        var result = await controller.UpdateCommandAsync(It.IsAny<Guid>(), item).ConfigureAwait(false);

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _verifyServiceMock.Verify(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()),
            commandActionId == null ? Times.Never() : Times.Once());
        _commandServiceMock.Verify(s => s.UpdateCommandAsync(It.IsAny<Guid>(), It.IsAny<CommandDto>()));
    }

    [Fact]
    public async Task UpdateCommandAsync_WhenCommandActionIdIsNotValid_ReturnsNotFound()
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        _verifyServiceMock.Setup(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Create<CommandDto>();

        // Act
        var result = await controller.UpdateCommandAsync(It.IsAny<Guid>(), item).ConfigureAwait(false);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _verifyServiceMock.Verify(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task UpdateCommandAsync_ShouldLogError()
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        var item = _fixture.Create<CommandDto>();

        // Act
        var result = await controller.UpdateCommandAsync(It.IsAny<Guid>(), item).ConfigureAwait(false);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task DeleteCommandAsync_WhenCommandIdIsNotValid_ReturnsNotFound()
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };

        // Act
        var result = await controller.DeleteCommandAsync(It.IsAny<Guid>()).ConfigureAwait(false);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task DeleteCommandAsync_ShouldReturnOk()
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };

        // Act
        var result = await controller.DeleteCommandAsync(It.IsAny<Guid>()).ConfigureAwait(false);

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _commandServiceMock.Verify(s => s.DeleteCommandAsync(It.IsAny<Guid>()));
    }

    [Fact]
    public async Task DeleteCommandAsync_ShouldLogError()
    {
        // Arrange
        _verifyServiceMock.Setup(s => s.VerifyCommandAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());
        var controller = new CommandController(_commandServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };

        // Act
        var result = await controller.DeleteCommandAsync(It.IsAny<Guid>()).ConfigureAwait(false);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }
}