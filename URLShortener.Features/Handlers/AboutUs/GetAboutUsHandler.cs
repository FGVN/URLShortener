using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URLShortener.Infrastructure.Data;
using URLShortener.Features.Requests;

namespace URLShortener.Features.Handlers;

public class GetAboutUsHandler : IRequestHandler<GetAboutUsRequest, IActionResult>
{
    private readonly AppDbContext _context;

    public GetAboutUsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Handle(GetAboutUsRequest request, CancellationToken cancellationToken)
    {
        var aboutUs = await _context.AboutUs
            .Include(a => a.Author)
            .FirstOrDefaultAsync();

        if (aboutUs == null)
        {
            return new NotFoundObjectResult("About Us content not found.");
        }

        return new OkObjectResult(aboutUs);
    }
}
