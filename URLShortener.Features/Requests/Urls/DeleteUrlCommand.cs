using MediatR;

namespace URLShortener.Features.Requests;

public class DeleteUrlCommand : IRequest<bool>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool IsAdmin { get; set; }
}
