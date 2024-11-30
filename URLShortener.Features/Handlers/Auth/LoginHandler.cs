using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URLShortener.Features.Requests;
using URLShortener.Services;
using URLShortener.Infrastructure.Data;

namespace URLShortener.Server.Handlers;

public class LoginHandler : IRequestHandler<LoginRequest, IActionResult>
{
    private readonly AppDbContext _context;
    private readonly EncryptionService _encryptionService;
    private readonly JwtTokenService _jwtTokenService;

    public LoginHandler(AppDbContext context, EncryptionService encryptionService, JwtTokenService jwtTokenService)
    {
        _context = context;
        _encryptionService = encryptionService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<IActionResult> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == request.Login);
        if (user == null || !_encryptionService.VerifyPassword(user.Password!, request.Password!))
        {
            return new UnauthorizedObjectResult("Invalid login or password.");
        }

        var token = _jwtTokenService.GenerateToken(user);
        return new OkObjectResult(new { Token = token });
    }
}
