using System.Security.Claims;
using AutoFixture;
using Contracts.Dto;
using Contracts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;
using UI.Controllers;
using Xunit;

namespace Tests.UnitTests.ControllersTests;

public class CommandActionControllerTests
{
    private readonly Mock<ICommandActionService> _commandActionServiceMock;
    private readonly ControllerContext _controllerContext;
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<CommandActionController>> _loggerMock;
    private readonly Mock<IVerifyService> _verifyServiceMock;

    public CommandActionControllerTests()
    {
        _commandActionServiceMock = new Mock<ICommandActionService>();
        _loggerMock = new Mock<ILogger<CommandActionController>>();
        _controllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
            }
        };
        _fixture = new Fixture();
        _verifyServiceMock = new Mock<IVerifyService>();
    }

    [Fact]
    public async Task GetCommandAction_ShouldReturnOk()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);

        // Act
        var result = await controller.GetCommandActionsByTelegramBotIdAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _commandActionServiceMock.Verify(s => s.GetCommandActionsByTelegramBotIdAsync(It.IsAny<Guid>()));
    }

    [Fact]
    public async Task GetCommandAction_ShouldReturnNotFound()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var result = await controller.GetCommandActionsByTelegramBotIdAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task GetCommandAction_ShouldLogError()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await controller.GetCommandActionsByTelegramBotIdAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task GetCommandActionByType_ShouldReturnOk()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);

        // Act
        var result =
            await controller.GetCommandActionsByTelegramBotIdAndActionTypeAsync(It.IsAny<Guid>(),
                It.IsAny<CommandActionType>());

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _commandActionServiceMock.Verify(s =>
            s.GetCommandActionsByTelegramBotIdAndActionTypeAsync(It.IsAny<Guid>(), It.IsAny<CommandActionType>()));
    }

    [Fact]
    public async Task GetCommandActionByType_ShouldReturnNotFound()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var result =
            await controller.GetCommandActionsByTelegramBotIdAndActionTypeAsync(It.IsAny<Guid>(),
                It.IsAny<CommandActionType>());

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task GetCommandActionByType_ShouldLogError()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var result =
            await controller.GetCommandActionsByTelegramBotIdAndActionTypeAsync(It.IsAny<Guid>(),
                It.IsAny<CommandActionType>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task CreateCommandAction_ShouldReturnOk()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var item = _fixture.Create<CommandActionDto>();

        // Act
        var result = await controller.CreateCommandActionAsync(item);

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _commandActionServiceMock.Verify(s => s.CreateCommandActionAsync(It.IsAny<CommandActionDto>()));
    }

    [Fact]
    public async Task CreateCommandAction_ShouldReturnNotFound()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        var item = _fixture.Create<CommandActionDto>();

        // Act
        var result = await controller.CreateCommandActionAsync(item);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task CreateCommandAction_ShouldLogError()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());
        var item = _fixture.Create<CommandActionDto>();

        // Act
        var result = await controller.CreateCommandActionAsync(item);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyTelegramBotAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task UpdateCommandAction_ShouldReturnOk()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var item = _fixture.Create<CommandActionDto>();

        // Act
        var result = await controller.UpdateCommandActionAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _commandActionServiceMock.Verify(
            s => s.UpdateCommandActionAsync(It.IsAny<Guid>(), It.IsAny<CommandActionDto>()));
    }

    [Fact]
    public async Task UpdateCommandAction_ShouldReturnNotFound()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);
        var item = _fixture.Create<CommandActionDto>();

        // Act
        var result = await controller.UpdateCommandActionAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task UpdateCommandAction_ShouldLogError()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());
        var item = _fixture.Create<CommandActionDto>();

        // Act
        var result = await controller.UpdateCommandActionAsync(It.IsAny<Guid>(), item);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public async Task DeleteCommandAction_ShouldReturnOk()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(true);

        // Act
        var result = await controller.DeleteCommandActionAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<OkResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _commandActionServiceMock.Verify(s => s.DeleteCommandActionAsync(It.IsAny<Guid>()));
    }

    [Fact]
    public async Task DeleteCommandAction_ShouldReturnNotFound()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var result = await controller.DeleteCommandActionAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
    }

    [Fact]
    public async Task DeleteCommandAction_ShouldLogError()
    {
        // Arrange
        var controller = new CommandActionController(_commandActionServiceMock.Object, _verifyServiceMock.Object,
            _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _verifyServiceMock.Setup(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await controller.DeleteCommandActionAsync(It.IsAny<Guid>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _verifyServiceMock.Verify(s => s.VerifyCommandActionAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Guid>()));
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }
}