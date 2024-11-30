using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace URLShortener.Features.Requests;

public class GetAboutUsRequest : IRequest<IActionResult>
{
}
