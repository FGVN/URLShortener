using MediatR;
using URLShortener.Core.Dtos;

namespace URLShortener.Features.Requests;

public class GetUrlsQuery : IRequest<PagedResult<UrlGlobalDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int? AuthorId { get; set; }
}
