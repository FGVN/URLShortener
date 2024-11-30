
namespace URLShortener.Server.Dtos;

public class UrlGlobalDto
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int? AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
}
