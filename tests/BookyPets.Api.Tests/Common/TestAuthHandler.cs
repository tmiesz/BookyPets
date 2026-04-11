using System.Security.Claims;
using System.Text.Encodings.Web;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Domain.Tests.TestConstants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookyPets.Api.Tests.Common;

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,

    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Test";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim> { new("id", Constants.Reader.Id.ToString()) };

        foreach (var permission in Constants.Reader.Permissions)
            claims.Add(new Claim("permissions", permission));

        foreach (var role in Constants.Reader.Roles)
            claims.Add(new Claim("roles", role));

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
