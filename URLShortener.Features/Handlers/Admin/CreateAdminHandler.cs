using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URLShortener.Core.Models;
using URLShortener.Features.Requests;
using Microsoft.Extensions.Configuration;
using URLShortener.Infrastructure.Data;
using URLShortener.Services;

namespace URLShortener.Features.Handlers;

public class CreateAdminHandler : IRequestHandler<CreateAdminRequest, IActionResult>
{
    private readonly AppDbContext _context;
    private readonly EncryptionService _encryptionService;
    private readonly string? _masterPassword;

    public CreateAdminHandler(AppDbContext context, IConfiguration configuration, EncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
        _masterPassword = configuration["AdminSettings:MasterPassword"];
    }

    public async Task<IActionResult> Handle(CreateAdminRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.MasterPassword) || request.MasterPassword != _masterPassword)
        {
            return new UnauthorizedObjectResult("Invalid master password.");
        }

        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new BadRequestObjectResult("Invalid user details.");
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
        if (existingUser != null)
        {
            return new ConflictObjectResult("User already exists.");
        }

        var newAdmin = new Admin(request.UserName, request.Login!, request.Password)
        {
            Password = _encryptionService.HashPassword(request.Password)
        };

        _context.Admins.Add(newAdmin);
        await _context.SaveChangesAsync();

        return new OkObjectResult(new { message = "Admin created successfully." });
    }
}
