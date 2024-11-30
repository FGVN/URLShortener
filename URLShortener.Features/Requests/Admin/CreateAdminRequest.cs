using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace URLShortener.Features.Requests;

public class CreateAdminRequest : IRequest<IActionResult>
{
    public string? UserName { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? MasterPassword { get; set; }
}
