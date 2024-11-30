using MediatR;
using Microsoft.EntityFrameworkCore;
using URLShortener.Features.Requests;
using URLShortener.Core.Dtos;
using URLShortener.Infrastructure.Data;

namespace URLShortener.Features.Urls;

public class GetUrlsQueryHandler : IRequestHandler<GetUrlsQuery, PagedResult<UrlGlobalDto>>
{
    private readonly AppDbContext _context;

    public GetUrlsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<UrlGlobalDto>> Handle(GetUrlsQuery request, CancellationToken cancellationToken)
    {
        if (request.Page < 1 || request.PageSize < 1)
        {
            throw new ArgumentException("Page and pageSize must be greater than zero.");
        }

        var query = _context.URLs.AsQueryable();

        if (request.AuthorId.HasValue)
        {
            query = query.Where(x => x.AuthorId == request.AuthorId.Value);
        }

        var totalRecords = await query.Where(x => !x.IsDeleted).CountAsync(cancellationToken);

        var urls = await query
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new UrlGlobalDto( x.Id, x.Url, x.AuthorId, x.CreatedAt ))
            .ToListAsync(cancellationToken);

        return new PagedResult<UrlGlobalDto>(request.Page, request.PageSize, totalRecords, urls);
    }
}
