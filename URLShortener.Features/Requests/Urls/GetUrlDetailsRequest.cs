using MediatR;
using URLShortener.Core.Dtos;

namespace URLShortener.Features.Requests;

public class GetUrlDetailsQuery : IRequest<UrlDetailsDto?>
{
    public int Id { get; set; }
}
