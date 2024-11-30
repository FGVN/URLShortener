using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using URLShortener.Core.Models;
using URLShortener.Features.Requests;
using URLShortener.Server.Handlers;
using URLShortener.Infrastructure.Data;
using URLShortener.Services;

namespace URLShortener.Features;

public class CreateShortenedUrlHandlerTests
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
    private readonly AppDbContext _dbContext;
    private readonly Mock<UrlMetadataService> _mockMetadataService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;
    private readonly CreateShortenedUrlHandler _handler;

    public CreateShortenedUrlHandlerTests()
    {
        // Configure an in-memory database for testing
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _dbContext = new AppDbContext(_dbContextOptions);

        _mockMetadataService = new Mock<UrlMetadataService>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockHttpContext = new Mock<HttpContext>();
        _mockClaimsPrincipal = new Mock<ClaimsPrincipal>();

        _mockHttpContext.Setup(x => x.User).Returns(_mockClaimsPrincipal.Object);
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);

        _handler = new CreateShortenedUrlHandler(
            _dbContext,
            _mockMetadataService.Object,
            _mockHttpContextAccessor.Object
        );
    }

    [Fact]
    public async Task Handle_MissingOriginUrlOrUrl_ReturnsBadRequestObjectResult()
    {
        // Arrange
        var request = new CreateUrlRequest { OriginUrl = "", Url = "" };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Original URL and shortened URL are required.", badRequestResult.Value);
    }

    [Fact]
    public async Task Handle_UrlAlreadyUsed_ReturnsBadRequestObjectResult()
    {
        // Arrange
        var request = new CreateUrlRequest
        {
            OriginUrl = "http://example.com",
            Url = "http://short.url"
        };

        _dbContext.URLs.Add(new URLData("http://example.com", "http://short.url", 1) { IsDeleted = false });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Sorry, that URL has already been used to shorten the link.", badRequestResult.Value);
    }

    [Fact]
    public async Task Handle_OriginUrlAlreadyShortened_ReturnsBadRequestObjectResult()
    {
        // Arrange
        var request = new CreateUrlRequest
        {
            OriginUrl = "http://example.com",
            Url = "http://another.url"
        };

        _dbContext.URLs.Add(new URLData("http://example.com", "http://existing.url", 1) { IsDeleted = false });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Sorry, that URL has already been shortened.", badRequestResult.Value);
    }
}
