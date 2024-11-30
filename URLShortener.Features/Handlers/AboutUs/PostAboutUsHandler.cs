using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using URLShortener.Features.Requests;
using URLShortener.Infrastructure.Data;

namespace URLShortener.Features.Handlers;

public class PostAboutUsHandler : IRequestHandler<PostAboutUsRequest, IActionResult>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostAboutUsHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> Handle(PostAboutUsRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return new BadRequestObjectResult("Content cannot be empty.");
        }

        var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        if (userId == 0)
        {
            return new UnauthorizedObjectResult("Invalid user ID.");
        }

        var aboutUsEntry = _context.AboutUs.First();
        aboutUsEntry.Content = request.Content;
        aboutUsEntry.AuthorId = userId;

        await _context.SaveChangesAsync();

        return new OkResult();
    }
}
