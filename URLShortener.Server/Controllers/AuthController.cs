using MediatR;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Features.Requests;

namespace URLShortener.Server.Controllers;

[ApiController]
[Route(GlobalConstants.ApiPathPrefix + "/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        return await _mediator.Send(request);
    }
}
