namespace URLShortener.Server.Models;

public class AuthorizedUser : User
{
    public AuthorizedUser() : base() 
    {
    }
    public AuthorizedUser(string username, string login, string password) : base(username, login, password)
    {
    }
}
