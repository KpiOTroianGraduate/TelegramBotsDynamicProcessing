using System.Linq.Expressions;
using System.Security.Claims;
using System.Transactions;
using AutoFixture;
using AutoMapper;
using Contracts.Entities;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using Xunit;

namespace Tests.UnitTests.ServicesTests;

public class VerifyServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<VerifyService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWorkFactory> _unitOfWorkFactoryMock;
    private readonly Mock<ISqlUnitOfWork> _unitOfWorkMock;

    public VerifyServiceTests()
    {
        _fixture = new Fixture();
        _mapperMock = new Mock<IMapper>();
        _unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        _unitOfWorkMock = new Mock<ISqlUnitOfWork>();
        _loggerMock = new Mock<ILogger<VerifyService>>();

        _unitOfWorkFactoryMock.Setup(x => x.CreateSqlUnitOfWork(It.IsAny<IsolationLevel>()))
            .Returns(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task VerifyTelegramBotAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .Without(u => u.TelegramBots)
            .Create();
        _mapperMock.Setup(x => x.Map<User>(It.IsAny<Claim[]>())).Returns(user);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        var verifyService = new VerifyService(_mapperMock.Object, _unitOfWorkFactoryMock.Object, _loggerMock.Object);

        // Act

        var result = await verifyService.VerifyTelegramBotAsync(Array.Empty<Claim>(), It.IsAny<Guid>());

        // Assert
        Assert.True(result);
        _mapperMock.Verify(m => m.Map<User>(It.IsAny<Claim[]>()));
        _unitOfWorkFactoryMock.Verify(u => u.CreateSqlUnitOfWork(It.IsAny<IsolationLevel>()));
        _unitOfWorkMock.Verify(u => u.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()));
    }

    [Fact]
    public async Task VerifyTelegramBotAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(x => x.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User?)null);
        var verifyService = new VerifyService(_mapperMock.Object, _unitOfWorkFactoryMock.Object, _loggerMock.Object);

        // Act
        var result = await verifyService.VerifyTelegramBotAsync(Array.Empty<Claim>(), It.IsAny<Guid>());

        // Assert
        Assert.False(result);
        _mapperMock.Verify(m => m.Map<User>(It.IsAny<Claim[]>()));
        _unitOfWorkFactoryMock.Verify(u => u.CreateSqlUnitOfWork(It.IsAny<IsolationLevel>()));
        _unitOfWorkMock.Verify(u => u.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()));
    }

    [Fact]
    public async Task VerifyCommandAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .Without(u => u.TelegramBots)
            .Create();
        _mapperMock.Setup(x => x.Map<User>(It.IsAny<Claim[]>())).Returns(user);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        var verifyService = new VerifyService(_mapperMock.Object, _unitOfWorkFactoryMock.Object, _loggerMock.Object);

        // Act
        var result = await verifyService.VerifyCommandAsync(Array.Empty<Claim>(), It.IsAny<Guid>());

        // Assert
        Assert.True(result);
        _mapperMock.Verify(m => m.Map<User>(It.IsAny<Claim[]>()));
        _unitOfWorkFactoryMock.Verify(u => u.CreateSqlUnitOfWork(It.IsAny<IsolationLevel>()));
        _unitOfWorkMock.Verify(u => u.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()));
    }

    [Fact]
    public async Task VerifyCommandAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(x => x.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User?)null);
        var verifyService = new VerifyService(_mapperMock.Object, _unitOfWorkFactoryMock.Object, _loggerMock.Object);

        // Act
        var result = await verifyService.VerifyCommandAsync(Array.Empty<Claim>(), It.IsAny<Guid>());

        // Assert
        Assert.False(result);
        _mapperMock.Verify(m => m.Map<User>(It.IsAny<Claim[]>()));
        _unitOfWorkFactoryMock.Verify(u => u.CreateSqlUnitOfWork(It.IsAny<IsolationLevel>()));
        _unitOfWorkMock.Verify(u => u.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()));
    }

    [Fact]
    public async Task VerifyCommandActionAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .Without(u => u.TelegramBots)
            .Create();
        _mapperMock.Setup(x => x.Map<User>(It.IsAny<Claim[]>())).Returns(user);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        var verifyService = new VerifyService(_mapperMock.Object, _unitOfWorkFactoryMock.Object, _loggerMock.Object);

        // Act
        var result = await verifyService.VerifyCommandActionAsync(Array.Empty<Claim>(), It.IsAny<Guid>());

        // Assert
        Assert.True(result);
        _mapperMock.Verify(m => m.Map<User>(It.IsAny<Claim[]>()));
        _unitOfWorkFactoryMock.Verify(u => u.CreateSqlUnitOfWork(It.IsAny<IsolationLevel>()));
        _unitOfWorkMock.Verify(u => u.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()));
    }

    [Fact]
    public async Task VerifyCommandActionAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(x => x.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User?)null);
        var verifyService = new VerifyService(_mapperMock.Object, _unitOfWorkFactoryMock.Object, _loggerMock.Object);

        // Act
        var result = await verifyService.VerifyCommandActionAsync(Array.Empty<Claim>(), It.IsAny<Guid>());

        // Assert
        Assert.False(result);
        _mapperMock.Verify(m => m.Map<User>(It.IsAny<Claim[]>()));
        _unitOfWorkFactoryMock.Verify(u => u.CreateSqlUnitOfWork(It.IsAny<IsolationLevel>()));
        _unitOfWorkMock.Verify(u => u.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()));
    }
}