using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using URLShortener.Server.Data;
using URLShortener.Server.Models;
using URLShortener.Server.Requests;

namespace URLShortener.Server.Controllers;

[Route("api/aboutus")]
[ApiController]
public class AboutUsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AboutUsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/aboutus
    [HttpGet]
    public async Task<IActionResult> GetAboutUs()
    {
        var aboutUs = await _context.AboutUs
            .Include(a => a.Author)
            .FirstOrDefaultAsync();

        if (aboutUs == null)
        {
            return NotFound("About Us content not found.");
        }

        return Ok(aboutUs);
    }

    // POST: api/aboutus
    [HttpPost]
    public async Task<IActionResult> PostAboutUs([FromBody] AboutUsUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("Content cannot be empty.");
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        if (userId == 0)
        {
            return Unauthorized("Invalid user ID.");
        }

        _context.AboutUs.First().Content = request.Content;
        _context.AboutUs.First().AuthorId = userId;

        await _context.SaveChangesAsync();

        return Ok();
    }
}
