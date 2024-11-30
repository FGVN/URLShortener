using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using URLShortener.Core.Models;
using URLShortener.Features.Requests;
using URLShortener.Infrastructure.Data;
using URLShortener.Services;

namespace URLShortener.Server.Handlers;

public class CreateShortenedUrlHandler : IRequestHandler<CreateUrlRequest, IActionResult>
{
    private readonly AppDbContext _context;
    private readonly UrlMetadataService _metadataService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateShortenedUrlHandler(AppDbContext context, UrlMetadataService metadataService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _metadataService = metadataService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> Handle(CreateUrlRequest request, CancellationToken cancellationToken)
    {
        var input = request;

        if (string.IsNullOrEmpty(input.OriginUrl) || string.IsNullOrEmpty(input.Url))
            return new BadRequestObjectResult("Original URL and shortened URL are required.");

        if (_context.URLs.Any(x => x.Url == input.Url && x.IsDeleted == false))
            return new BadRequestObjectResult("Sorry, that URL has already been used to shorten the link.");

        if (_context.URLs.Any(x => x.OriginUrl == input.OriginUrl && x.IsDeleted == false))
            return new BadRequestObjectResult("Sorry, that URL has already been shortened.");

        // Access HttpContext from the IHttpContextAccessor
        var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var urlData = new URLData(input.OriginUrl, input.Url, userId);

        var metadata = await _metadataService.FetchMetadataAsync(input.OriginUrl);
        if (metadata != null)
        {
            urlData.UpdateMetadata(metadata);
        }

        _context.URLs.Add(urlData);
        await _context.SaveChangesAsync();

        return new OkObjectResult(urlData);
    }
}
