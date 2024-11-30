using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using URLShortener.Features.Requests;
using URLShortener.Infrastructure.Data;
using URLShortener.Services;
using URLShortener.Core.Models;


namespace URLShortener.Features.Handlers;

public class RegisterHandlerTests
{
    private readonly AppDbContext _dbContext;
    private readonly Mock<EncryptionService> _mockEncryptionService;
    private readonly Mock<JwtTokenService> _mockJwtTokenService;
    private readonly RegisterHandler _handler;

    public RegisterHandlerTests()
    {
        // Set up the in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new AppDbContext(options);

        // Mock services
        _mockEncryptionService = new Mock<EncryptionService>();
        _mockJwtTokenService = new Mock<JwtTokenService>();

        _handler = new RegisterHandler(_dbContext, _mockEncryptionService.Object, _mockJwtTokenService.Object);
    }

    [Fact]
    public async Task Handle_UserNameOrLoginAlreadyTaken_ReturnsBadRequestObjectResult()
    {
        // Arrange
        _dbContext.AuthorizedUsers.Add(new AuthorizedUser("existingUser", "existingLogin", "hashedPassword"));
        await _dbContext.SaveChangesAsync();

        var request = new RegisterRequest
        {
            UserName = "existingUser",
            Login = "newLogin",
            Password = "password123"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username or login is already taken.", badRequestResult.Value);
    }

    [Fact]
    public async Task Handle_UserNameOrLoginAlreadyTaken_ReturnsBadRequestObjectResult_IfLoginTaken()
    {
        // Arrange
        _dbContext.AuthorizedUsers.Add(new AuthorizedUser("anotherUser", "existingLogin", "hashedPassword"));
        await _dbContext.SaveChangesAsync();

        var request = new RegisterRequest
        {
            UserName = "newUserName",
            Login = "existingLogin",
            Password = "password123"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username or login is already taken.", badRequestResult.Value);
    }
}
