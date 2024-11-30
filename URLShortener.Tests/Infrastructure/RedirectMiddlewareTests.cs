using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using URLShortener.Infrastructure.Data;
using URLShortener.Infrastructure.MiddleWares;
using URLShortener.Core.Models;
using System;
using System.Net;

namespace URLShortener.Tests.Infrastructure;

public class RedirectMiddlewareTests
{
    private readonly Mock<AppDbContext> _mockDbContext;
    private readonly RedirectMiddleware _middleware;
    private readonly DefaultHttpContext _context;

    public RedirectMiddlewareTests()
    {
        _mockDbContext = new Mock<AppDbContext>();
        _middleware = new RedirectMiddleware(next => Task.CompletedTask);
        _context = new DefaultHttpContext();
    }

    [Fact]
    public async Task InvokeAsync_ShouldNotRedirect_WhenPathStartsWithApiPrefix()
    {
        // Arrange
        _context.Request.Path = "/api/some-path";
        _context.RequestServices = new ServiceCollection()
            .AddScoped(_ => _mockDbContext.Object)
            .BuildServiceProvider();
        var nextMiddlewareCalled = false;

        RequestDelegate next = (context) =>
        {
            nextMiddlewareCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new RedirectMiddleware(next);

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        Assert.True(nextMiddlewareCalled);
    }

}
