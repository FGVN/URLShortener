﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using URLShortener.Core.Models;

namespace URLShortener.Services;

public class JwtTokenService
{
    private readonly string? _issuer;
    private readonly string? _audience;
    private readonly string? _secretKey;

    public JwtTokenService()
    {
    }

    public JwtTokenService(string issuer, string audience, string secretKey)
    {
        _issuer = issuer;
        _audience = audience;
        _secretKey = secretKey;
    }
    public string GenerateToken(User user, int expirationMinutes = 60)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        if (user is Admin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admins"));
        }
        else if (user is AuthorizedUser)
        {
            claims.Add(new Claim(ClaimTypes.Role, "AuthorizedUsers"));
        }

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
