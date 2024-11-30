using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using URLShortener.Server.Data;
using URLShortener.Server.Models;
using URLShortener.Server.Requests;
using URLShortener.Server.Services;

namespace URLShortener.Server.Controllers;

[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly EncryptionService _encryptionService;
    private readonly string? _masterPassword;

    public AdminController(AppDbContext context, IConfiguration configuration, EncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
        _masterPassword = configuration["AdminSettings:MasterPassword"];
        if (_masterPassword is null)
            Console.WriteLine("MASTER PASSWORD NOT FOUND IN APPSETTINGS\nNOTE THAT NEW ADDMINS CANNOT BE ADDED");
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAdmin([FromBody] RegisterRequest newAdminRequest, [FromHeader] string masterPassword)
    {
        Admin newAdmin = new Admin(newAdminRequest.UserName, newAdminRequest.Login, newAdminRequest.Password);

        if (string.IsNullOrWhiteSpace(masterPassword) || masterPassword != _masterPassword)
        {
            return Unauthorized("Invalid master password.");
        }

        if (newAdmin == null || string.IsNullOrWhiteSpace(newAdmin.UserName) || string.IsNullOrWhiteSpace(newAdmin.Password))
        {
            return BadRequest("Invalid user details.");
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == newAdmin.UserName);
        if (existingUser != null)
        {
            return Conflict("User already exists.");
        }

        newAdmin.Password = _encryptionService.HashPassword(newAdminRequest.Password);

        _context.Admins.Add(newAdmin);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Admin created successfully." });
    }
}
