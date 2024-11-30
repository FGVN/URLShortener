using System;
using Xunit;
using URLShortener.Services;

namespace URLShortener.Tests.Services;

public class EncryptionServiceTests
{
    private readonly EncryptionService _encryptionService;

    public EncryptionServiceTests()
    {
        _encryptionService = new EncryptionService();
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = "testPassword";

        // Act
        var hashedPassword = _encryptionService.HashPassword(password);

        // Assert
        Assert.False(string.IsNullOrEmpty(hashedPassword));
        Assert.NotEqual(password, hashedPassword); // Ensures that the password is actually hashed
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
    {
        // Arrange
        var password = "testPassword";
        var hashedPassword = _encryptionService.HashPassword(password);

        // Act
        var isPasswordValid = _encryptionService.VerifyPassword(hashedPassword, password);

        // Assert
        Assert.True(isPasswordValid);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
    {
        // Arrange
        var password = "testPassword";
        var hashedPassword = _encryptionService.HashPassword(password);
        var incorrectPassword = "wrongPassword";

        // Act
        var isPasswordValid = _encryptionService.VerifyPassword(hashedPassword, incorrectPassword);

        // Assert
        Assert.False(isPasswordValid);
    }
}
