using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using URLShortener.Features.Requests;
using URLShortener.Features.Handlers;
using URLShortener.Infrastructure.Data;

namespace URLShortener.Tests.Features;

public class PostAboutUsHandlerTests
{
    private readonly Mock<AppDbContext> _mockDbContext;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly PostAboutUsHandler _handler;

    public PostAboutUsHandlerTests()
    {
        _mockDbContext = new Mock<AppDbContext>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _handler = new PostAboutUsHandler(_mockDbContext.Object, _mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequestObjectResult_WhenContentIsEmpty()
    {
        // Arrange
        var request = new PostAboutUsRequest { Content = "" };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Content cannot be empty.", badRequestResult.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorizedObjectResult_WhenUserIdIsInvalid()
    {
        // Arrange
        var request = new PostAboutUsRequest { Content = "Valid Content" };

        // Set up mock HTTP context with an invalid user ID
        _mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(new DefaultHttpContext());
        _mockHttpContextAccessor.Object.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "0")
        }));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid user ID.", unauthorizedResult.Value);
    }
}
