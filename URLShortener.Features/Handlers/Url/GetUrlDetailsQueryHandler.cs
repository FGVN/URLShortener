using MediatR;
using Microsoft.EntityFrameworkCore;
using URLShortener.Core.Dtos;
using URLShortener.Features.Requests;
using URLShortener.Infrastructure.Data;

namespace URLShortener.Server.Features.Urls;

public class GetUrlDetailsQueryHandler : IRequestHandler<GetUrlDetailsQuery, UrlDetailsDto?>
{
    private readonly AppDbContext _context;

    public GetUrlDetailsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UrlDetailsDto?> Handle(GetUrlDetailsQuery request, CancellationToken cancellationToken)
    {
        var url = await _context.URLs
            .Include(x => x.Metadata)
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (url == null)
        {
            return null; 
        }

        return new UrlDetailsDto
        (
            url.Id,
            url.Url,
            url.OriginUrl,
            url.CreatedAt,
            url.Metadata,
            url.Author?.UserName,
            url.AuthorId
        );
    }
}
