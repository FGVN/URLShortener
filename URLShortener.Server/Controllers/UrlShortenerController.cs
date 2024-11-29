using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using URLShortener.Server.Data;
using URLShortener.Server.Dtos;
using URLShortener.Server.Models;
using URLShortener.Server.Services;

namespace URLShortener.Server.Controllers;

[ApiController]
[Route(GlobalConstants.ApiPathPrefix+"/[controller]")]
public class UrlShortenerController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UrlMetadataService _metadataService;

    public UrlShortenerController(AppDbContext context, UrlMetadataService metadataService)
    {
        _context = context;
        _metadataService = metadataService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateShortenedUrl([FromBody] CreateUrlRequest request)
    {
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim type: {claim.Type}, Value: {claim.Value}");
        }

        if (string.IsNullOrEmpty(request.OriginUrl) || string.IsNullOrEmpty(request.Url))
            return BadRequest("Original URL and shortened URL are required.");

        if (_context.URLs.Any(x => x.Url == request.Url && x.IsDeleted == false))
            return BadRequest("Sorry, that URL has already been used to shorten the link.");

        if (_context.URLs.Any(x => x.OriginUrl == request.OriginUrl && x.IsDeleted == false))
            return BadRequest("Sorry, that URL has already been shortened.");

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        Console.WriteLine("ID: " + userId);
        var urlData = new URLData(request.OriginUrl, request.Url, userId);

        var metadata = await _metadataService.FetchMetadataAsync(request.OriginUrl);
        if (metadata != null)
        {
            urlData.UpdateMetadata(metadata);
        }

        _context.URLs.Add(urlData);
        await _context.SaveChangesAsync();

        return Ok(urlData);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUrls([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int? authorId = null)
    {
        if (page < 1 || pageSize < 1)
            return BadRequest("Page and pageSize must be greater than zero.");

        var query = _context.URLs.AsQueryable();

        if (authorId.HasValue)
        {
            query = query.Where(x => x.AuthorId == authorId.Value);
        }

        var totalRecords = await query.CountAsync();
        var urls = await query
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new UrlGlobalDto
            {
                Id = x.Id,
                Url = x.Url,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();

        var result = new PagedResult<UrlGlobalDto>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            Data = urls
        };

        return Ok(result);
    }


    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUrlDetails(int id)
    {
        var url = await _context.URLs
            .Include(x => x.Metadata)
            .Include(x => x.Author) 
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (url == null)
            return NotFound("URL not found.");

        var urlDetailsDto = new UrlDetailsDto
        {
            Id = url.Id,
            Url = url.Url,
            OriginUrl = url.OriginUrl,
            CreatedAt = url.CreatedAt,
            Metadata = url.Metadata,
            Username = url.Author?.UserName 
        };

        return Ok(urlDetailsDto);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUrl(int id)
    {
        var url = await _context.URLs
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (url == null || url.IsDeleted)
            return NotFound("URL not found or already deleted.");

        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var isAdmin = User.IsInRole("Admin");

        if (url.AuthorId != userId && !isAdmin)
            return Forbid("You are not authorized to delete this URL.");

        url.IsDeleted = true;
        await _context.SaveChangesAsync();

        return Ok("The URL has been marked as deleted.");
    }


    [HttpGet("redirect/{shortUrl}")]
    public async Task<IActionResult> RedirectToOriginal(string shortUrl)
    {
        var url = await _context.URLs.FirstOrDefaultAsync(x => x.Url == shortUrl && !x.IsDeleted);

        if (url == null)
            return NotFound("The URL has been deleted or does not exist.");

        return Redirect(url.OriginUrl);
    }

}
