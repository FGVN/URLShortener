using MediatR;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Core.Models;
using URLShortener.Features.Requests;
using URLShortener.Infrastructure.Data;
using URLShortener.Services;

namespace URLShortener.Server.Handlers;

public class RegisterHandler : IRequestHandler<RegisterRequest, IActionResult>
{
    private readonly AppDbContext _context;
    private readonly EncryptionService _encryptionService;
    private readonly JwtTokenService _jwtTokenService;

    public RegisterHandler(AppDbContext context, EncryptionService encryptionService, JwtTokenService jwtTokenService)
    {
        _context = context;
        _encryptionService = encryptionService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<IActionResult> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        if (_context.Users.Any(x => x.UserName == request.UserName || x.Login == request.Login))
            return new BadRequestObjectResult("Username or login is already taken.");

        var hashedPassword = _encryptionService.HashPassword(request.Password!);
        var user = new AuthorizedUser(request.UserName!, request.Login!, hashedPassword!);

        _context.AuthorizedUsers.Add(user);
        await _context.SaveChangesAsync();

        var token = _jwtTokenService.GenerateToken(user);
        return new OkObjectResult(new { Token = token });
    }
}
