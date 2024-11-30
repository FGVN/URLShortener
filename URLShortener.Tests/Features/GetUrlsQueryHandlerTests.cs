using Microsoft.EntityFrameworkCore;
using URLShortener.Features.Requests;
using URLShortener.Infrastructure.Data;
using URLShortener.Features.Urls;
using URLShortener.Core.Models;

namespace URLShortener.Tests.Features;

public class GetUrlsQueryHandlerTests
{
    private readonly AppDbContext _dbContext;
    private readonly GetUrlsQueryHandler _handler;

    public GetUrlsQueryHandlerTests()
    {
        // Set up the in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new AppDbContext(options);

        _handler = new GetUrlsQueryHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsPagedResultWithCorrectTotalCount()
    {
        // Arrange

        _dbContext.URLs.RemoveRange(_dbContext.URLs);
        _dbContext.URLs.AddRange(
            new URLData ( "http://example1.com", "1", 1),
            new URLData ( "http://example1.com", "2", 2),
            new URLData ( "http://example1.com", "3", 6)
        );
        await _dbContext.SaveChangesAsync();

        var request = new GetUrlsQuery
        {
            Page = 1,
            PageSize = 2,
            AuthorId = 1
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(1, result.TotalRecords); // Only one URLData by author 1 should be counted as the other is deleted.
        Assert.Single(result.Data);
        Assert.Equal("1", result.Data.First().Url);
    }

    [Fact]
    public async Task Handle_NoMatchingURLDatas_ReturnsEmptyPagedResult()
    {
        // Arrange
        var request = new GetUrlsQuery
        {
            Page = 1,
            PageSize = 2,
            AuthorId = 3 // No matching author
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(0, result.TotalRecords);
        Assert.Empty(result.Data);
    }

    [Fact]
    public async Task Handle_InvalidPageOrPageSize_ThrowsArgumentException()
    {
        // Arrange
        var request = new GetUrlsQuery
        {
            Page = 0, // Invalid page
            PageSize = 2
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_HandlesDeletedURLDatasCorrectly()
    {
        // Arrange
        _dbContext.URLs.RemoveRange(_dbContext.URLs);
        _dbContext.URLs.AddRange(
            new URLData("http://example1.com", "1", 1),
            new URLData("http://example1.com", "2", 3)
        );
        await _dbContext.SaveChangesAsync();

        var request = new GetUrlsQuery
        {
            Page = 1,
            PageSize = 2,
            AuthorId = 1
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.TotalRecords); // The deleted URLData should not be counted
        Assert.Single(result.Data);
        Assert.Equal("1", result.Data.Last().Url);
    }
}
