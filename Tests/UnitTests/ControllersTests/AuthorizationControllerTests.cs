using System.Security.Claims;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;
using UI.Controllers;
using Xunit;

namespace Tests.UnitTests.ControllersTests;

public class AuthorizationControllerTests
{
    private readonly ControllerContext _controllerContext;
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<AuthorizationController>> _loggerMock;

    private readonly Mock<IUserService> _userServiceMock;

    public AuthorizationControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _loggerMock = new Mock<ILogger<AuthorizationController>>();
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
    public async Task Registration_ShouldReturnOk()
    {
        // Arrange
        var controller = new AuthorizationController(_userServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };

        // Act
        var result = await controller.Registration();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _userServiceMock.Verify(s => s.RegisterUserAsync(It.IsAny<IEnumerable<Claim>>()));
    }

    [Fact]
    public async Task Registration_ShouldLogError()
    {
        // Arrange
        var controller = new AuthorizationController(_userServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };
        _userServiceMock.Setup(s => s.RegisterUserAsync(It.IsAny<IEnumerable<Claim>>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await controller.Registration();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _userServiceMock.Verify(s => s.RegisterUserAsync(It.IsAny<IEnumerable<Claim>>()));
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));
    }

    [Fact]
    public void Login_ShouldReturnOk()
    {
        // Arrange
        var controller = new AuthorizationController(_userServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = _controllerContext
        };

        // Act
        var result = controller.LogIn();

        // Assert
        Assert.IsType<RedirectResult>(result);
    }
}