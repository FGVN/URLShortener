using URLShortener.Server.Services;

namespace URLShortener.Server.Models;

public class URLData
{
    public int Id { get; set; } 
    public string OriginUrl { get; set; }
    public string Url { get; set; }
    public int AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public UrlMetadata? Metadata { get; set; }
    public DateTime? LastFetchedAt { get; set; }

    public User Author { get; set; } = null!;

    public URLData(string originUrl, string url, int authorId)
    {
        OriginUrl = originUrl;
        Url = url;
        AuthorId = authorId;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void UpdateMetadata(UrlMetadata newData)
    {
        Metadata = newData;
        LastFetchedAt = DateTime.UtcNow;
    }
}
