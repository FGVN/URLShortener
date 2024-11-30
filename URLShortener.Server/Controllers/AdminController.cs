using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Features.Requests;

namespace URLShortener.Server.Controllers;

[Route("api/admin")]
[ApiController]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminRequest newAdminRequest, [FromHeader] string masterPassword)
    {
        newAdminRequest.MasterPassword = masterPassword;
        return await _mediator.Send(newAdminRequest);
    }
}
