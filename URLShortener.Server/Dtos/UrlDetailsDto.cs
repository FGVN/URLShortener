using URLShortener.Server.Services;

namespace URLShortener.Server.Dtos;

public class UrlDetailsDto
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string OriginUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public UrlMetadata? Metadata { get; set; }
    public string? Username { get; set; }
    public int? AuthorId { get; set; }
}
