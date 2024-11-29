using Microsoft.AspNetCore.Mvc;
using URLShortener.Server.Auth;
using URLShortener.Server.Data;
using URLShortener.Server.Models;
using URLShortener.Server.Services;
using URLShortener.Server.Requests;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly AppDbContext _context;
    private readonly EncryptionService _encryptionService;

    public AuthController(JwtTokenService jwtTokenService, AppDbContext context, EncryptionService encryptionService)
    {
        _jwtTokenService = jwtTokenService;
        _context = context;
        _encryptionService = encryptionService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (_context.Users.Any(x => x.UserName == request.UserName || x.Login == request.Login))
            return BadRequest("Username or login is already taken.");

        var hashedPassword = _encryptionService.HashPassword(request.Password);

        var user = new AuthorizedUser(request.UserName, request.Login, hashedPassword);
        _context.AuthorizedUsers.Add(user);
        _context.SaveChanges();

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.Users.FirstOrDefault(x => x.Login == request.Login);
        if (user == null || !_encryptionService.VerifyPassword(user.Password, request.Password))
        {
            return Unauthorized("Invalid login or password.");
        }

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }
}

