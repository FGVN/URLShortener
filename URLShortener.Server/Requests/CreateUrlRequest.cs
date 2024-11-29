namespace URLShortener.Server.Controllers;

public class CreateUrlRequest
{
    public string OriginUrl { get; set; }
    public string Url { get; set; }
}
