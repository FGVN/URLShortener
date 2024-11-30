namespace URLShortener.Server.Requests;

public class AboutUsUpdateRequest
{
    public string Content { get; set; }

    public AboutUsUpdateRequest(string content)
    {
        Content = content;
    }
}
