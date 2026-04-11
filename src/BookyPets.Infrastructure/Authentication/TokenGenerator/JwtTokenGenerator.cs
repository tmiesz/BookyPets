using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Infrastructure.Authentication.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookyPets.Infrastructure.Authentication.TokenGenerator;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(Reader reader)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, reader.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, reader.LastName),
            new(JwtRegisteredClaimNames.Email, reader.Email),
            new("id", reader.Id.ToString()),
        };

        foreach (var permission in reader.AccountType.GetPermissions())
            claims.Add(new Claim("permissions", permission));

        foreach (var role in reader.GetRoles())
            claims.Add(new Claim("roles", role));

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
