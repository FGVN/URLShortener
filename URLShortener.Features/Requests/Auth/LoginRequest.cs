using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace URLShortener.Features.Requests;

public class LoginRequest : IRequest<IActionResult>
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
    }

