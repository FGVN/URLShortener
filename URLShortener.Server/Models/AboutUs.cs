namespace URLShortener.Server.Models;

public class AboutUs
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int AuthorId { get; set; }
    public DateTime LastChangedAt { get; set; }
    public User? Author { get; set; } 

    public AboutUs() {}

    public AboutUs(string content, int auhtorId)
    {
        Content = content;
        AuthorId = auhtorId;
        LastChangedAt = DateTime.UtcNow; 
    }
}

