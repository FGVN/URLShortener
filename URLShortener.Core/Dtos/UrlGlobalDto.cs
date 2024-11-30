namespace URLShortener.Core.Dtos;

public class UrlGlobalDto
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int? AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }

    public UrlGlobalDto(int id, string url, int? authorId, DateTime createdAt)
    {
        Id = id;
        Url = url;
        AuthorId = authorId;
        CreatedAt = createdAt;
    }
}
