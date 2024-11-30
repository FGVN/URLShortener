using Microsoft.AspNetCore.Identity;

namespace URLShortener.Services;

public class EncryptionService
{
    private readonly PasswordHasher<string> _passwordHasher;

    public EncryptionService()
    {
        _passwordHasher = new PasswordHasher<string>();
    }

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(string.Empty, password);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(string.Empty, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
