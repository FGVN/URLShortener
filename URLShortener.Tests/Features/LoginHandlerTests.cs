using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using URLShortener.Core.Models;
using URLShortener.Features.Requests;
using URLShortener.Infrastructure.Data;
using URLShortener.Server.Handlers;
using URLShortener.Services;
using Xunit;

namespace URLShortener.Tests.Features;
public class LoginHandlerTests
{
    private readonly AppDbContext _dbContext;
    private readonly Mock<EncryptionService> _mockEncryptionService;
    private readonly Mock<JwtTokenService> _mockJwtTokenService;
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        // Set up the in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new AppDbContext(options);

        // Seed the database with test data
        _dbContext.Users.Add(new AuthorizedUser
        {
            Login = "testuser",
            Password = "hashedPassword" // Assume this is a pre-hashed password.
        });
        _dbContext.SaveChanges();

        // Mock services
        _mockEncryptionService = new Mock<EncryptionService>();
        _mockJwtTokenService = new Mock<JwtTokenService>();

        _handler = new LoginHandler(_dbContext, _mockEncryptionService.Object, _mockJwtTokenService.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsUnauthorizedObjectResult()
    {
        // Arrange
        var request = new LoginRequest
        {
            Login = "nonexistentuser",
            Password = "password123"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid login or password.", unauthorizedResult.Value);
    }
}
