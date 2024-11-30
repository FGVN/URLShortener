using MediatR;
using Microsoft.EntityFrameworkCore;
using URLShortener.Features.Requests;
using URLShortener.Infrastructure.Data;

namespace URLShortener.Server.Features.Urls;

public class DeleteUrlCommandHandler : IRequestHandler<DeleteUrlCommand, bool>
{
    private readonly AppDbContext _context;

    public DeleteUrlCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteUrlCommand request, CancellationToken cancellationToken)
    {
        var url = await _context.URLs
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (url == null || url.IsDeleted)
        {
            return false;
        }

        if (url.AuthorId != request.UserId && !request.IsAdmin)
        {
            return false; 
        }

        url.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);

        return true; 
    }
}
