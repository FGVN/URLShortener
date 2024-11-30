using Microsoft.AspNetCore.Mvc;
using Moq;
using URLShortener.Features.Requests;
using URLShortener.Infrastructure.Data;
using URLShortener.Services;
using Microsoft.Extensions.Configuration;
using URLShortener.Features.Handlers;

namespace URLShortener.Tests.Features;

public class CreateAdminHandlerTests
{
    private readonly Mock<AppDbContext> _mockDbContext;
    private readonly Mock<EncryptionService> _mockEncryptionService;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly CreateAdminHandler _handler;

    public CreateAdminHandlerTests()
    {
        _mockDbContext = new Mock<AppDbContext>();
        _mockEncryptionService = new Mock<EncryptionService>();
        _mockConfiguration = new Mock<IConfiguration>();

        // Setup configuration to return a known master password.
        _mockConfiguration.Setup(c => c["AdminSettings:MasterPassword"]).Returns("SecretPassword");

        _handler = new CreateAdminHandler(_mockDbContext.Object, _mockConfiguration.Object, _mockEncryptionService.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorizedObjectResult_WhenMasterPasswordIsInvalid()
    {
        // Arrange
        var request = new CreateAdminRequest
        {
            MasterPassword = "WrongPassword",
            UserName = "newAdmin",
            Password = "password123"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid master password.", unauthorizedResult.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequestObjectResult_WhenUserNameOrPasswordIsEmpty()
    {
        // Arrange
        var request = new CreateAdminRequest
        {
            MasterPassword = "SecretPassword",
            UserName = "",
            Password = "password123"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid user details.", badRequestResult.Value);
    }
}
