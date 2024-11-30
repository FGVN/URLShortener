using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace URLShortener.Features.Requests;

public class PostAboutUsRequest : IRequest<IActionResult>
{
    public string? Content { get; set; }
}
