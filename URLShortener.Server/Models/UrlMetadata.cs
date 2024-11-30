using System.Text.Json.Serialization;

namespace URLShortener.Server.Services;

public class UrlMetadata
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("image")]
    public string? ImageUrl { get; set; }
}
