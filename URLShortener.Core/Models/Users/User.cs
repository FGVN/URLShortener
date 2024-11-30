namespace URLShortener.Core.Models;

public abstract class User
{
    public int Id { get; set; } 
    public string? UserName { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }

    public User() 
    {
    }

    public User(string username, string login, string password)
    {
        UserName = username;    
        Login = login;
        Password = password;
    }
}
