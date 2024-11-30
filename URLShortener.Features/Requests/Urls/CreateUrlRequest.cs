using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace URLShortener.Features.Requests;

public class CreateUrlRequest : IRequest<IActionResult>
{
    public string? OriginUrl { get; set; }
    public string? Url { get; set; }
}
