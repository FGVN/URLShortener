using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using URLShortener.Core.Models;
using URLShortener.Services;

namespace URLShortener.Tests.Services;

public class JwtTokenServiceTests
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly string _secretKey = "29b84a00db7aa105b822c9853795bcc267538850dad6e53012e9f04decfd7ffc"; // Use a test secret key

    public JwtTokenServiceTests()
    {
        _jwtTokenService = new JwtTokenService("testIssuer", "testAudience", _secretKey);
    }

    [Fact]
    public void GenerateToken_ShouldCreateTokenForAdminUser()
    {
        // Arrange
        var adminUser = new Admin { Id = 1, UserName = "AdminUser" };

        // Act
        var token = _jwtTokenService.GenerateToken(adminUser);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.NotNull(jwtToken);
        Assert.Contains(jwtToken.Claims, claim => claim.Type == ClaimTypes.Role && claim.Value == "Admins");
        Assert.Equal("AdminUser", jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value);
        Assert.Equal("1", jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value);
    }

    [Fact]
    public void GenerateToken_ShouldCreateTokenForAuthorizedUser()
    {
        // Arrange
        var authorizedUser = new AuthorizedUser { Id = 2, UserName = "AuthorizedUser" };

        // Act
        var token = _jwtTokenService.GenerateToken(authorizedUser);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.NotNull(jwtToken);
        Assert.Contains(jwtToken.Claims, claim => claim.Type == ClaimTypes.Role && claim.Value == "AuthorizedUsers");
        Assert.Equal("AuthorizedUser", jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value);
        Assert.Equal("2", jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value);
    }

    [Fact]
    public void GenerateToken_ShouldIncludeExpectedClaims()
    {
        // Arrange
        var user = new AuthorizedUser { Id = 3, UserName = "TestUser" };

        // Act
        var token = _jwtTokenService.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        Assert.Contains(jwtToken.Claims, claim => claim.Type == JwtRegisteredClaimNames.Sub && claim.Value == "3");
        Assert.Contains(jwtToken.Claims, claim => claim.Type == JwtRegisteredClaimNames.UniqueName && claim.Value == "TestUser");
        Assert.Contains(jwtToken.Claims, claim => claim.Type == JwtRegisteredClaimNames.Jti);
    }

    [Fact]
    public void GenerateToken_ShouldSetExpirationTime()
    {
        // Arrange
        var user = new AuthorizedUser { Id = 4, UserName = "UserWithExpiration" };

        // Act
        var token = _jwtTokenService.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        Assert.NotNull(jwtToken.ValidTo);
        Assert.True(jwtToken.ValidTo > DateTime.UtcNow, "Token expiration time should be in the future");
        Assert.True((jwtToken.ValidTo - DateTime.UtcNow).TotalMinutes <= 60, "Token expiration should match the default 60 minutes");
    }
}
