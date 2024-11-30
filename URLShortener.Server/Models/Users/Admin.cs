namespace URLShortener.Server.Models;

public class Admin : User
{
    public Admin() : base() 
    {
    }
    public Admin(string username, string login, string password) : base(username, login, password)
    {
    }
}
