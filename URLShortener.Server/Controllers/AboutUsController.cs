using MediatR;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Features.Requests;

namespace URLShortener.Server.Controllers;

[Route("api/aboutus")]
[ApiController]
public class AboutUsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AboutUsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/aboutus
    [HttpGet]
    public async Task<IActionResult> GetAboutUs()
    {
        var request = new GetAboutUsRequest();
        return await _mediator.Send(request);
    }

    // POST: api/aboutus
    [HttpPost]
    public async Task<IActionResult> PostAboutUs([FromBody] PostAboutUsRequest request)
    {
        return await _mediator.Send(request);
    }
}
