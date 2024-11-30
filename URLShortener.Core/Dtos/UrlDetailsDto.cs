using URLShortener.Core.Models;

namespace URLShortener.Core.Dtos;

public class UrlDetailsDto
{

    public int Id { get; set; }
    public string Url { get; set; }
    public string OriginUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public UrlMetadata? Metadata { get; set; }
    public string? Username { get; set; }
    public int? AuthorId { get; set; }

    public UrlDetailsDto(int id, string url, string originUrl, DateTime createdAt, UrlMetadata? metadata, string? username, int? authorId)
    {
        Id = id;
        Url = url;
        OriginUrl = originUrl;
        CreatedAt = createdAt;
        Metadata = metadata;
        Username = username;
        AuthorId = authorId;
    }
}
