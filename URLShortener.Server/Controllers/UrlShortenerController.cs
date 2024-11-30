using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using URLShortener.Features.Requests;

namespace URLShortener.Server.Controllers;

[ApiController]
[Route(GlobalConstants.ApiPathPrefix+"/[controller]")]
public class UrlShortenerController : ControllerBase
{
    private readonly IMediator _mediator;

    public UrlShortenerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateShortenedUrl([FromBody] CreateUrlRequest request)
    {
        var result = await _mediator.Send(request);
        return result;
    }

    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetUrls([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int? authorId = null)
    {
        var query = new GetUrlsQuery
        {
            Page = page,
            PageSize = pageSize,
            AuthorId = authorId
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetUrlDetails(int id)
    {
        var query = new GetUrlDetailsQuery { Id = id };
        var urlDetails = await _mediator.Send(query);

        if (urlDetails == null)
        {
            return NotFound("URL not found.");
        }

        return Ok(urlDetails);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUrl(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var isAdmin = User.IsInRole("Admin");

        var command = new DeleteUrlCommand
        {
            Id = id,
            UserId = userId,
            IsAdmin = isAdmin
        };

        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound("URL not found or already deleted, or you are not authorized to delete this URL.");
        }

        return Ok("The URL has been marked as deleted.");
    }
}
