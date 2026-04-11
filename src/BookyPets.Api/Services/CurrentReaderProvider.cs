using BookyPets.Application.Common.Interfaces;
using BookyPets.Application.Common.Models;

namespace BookyPets.Api.Services;

public class CurrentReaderProvider(IHttpContextAccessor _httpContextAccessor) : ICurrentReaderProvider
{
    public CurrentReader GetCurrentReader()
    {
        if (_httpContextAccessor.HttpContext is null)
            throw new InvalidOperationException("No active HTTP context.");

        var id = GetClaimValues("id")
            .Select(Guid.Parse)
            .First();

        var permissions = GetClaimValues("permissions");
        var roles = GetClaimValues("roles");

        return new CurrentReader(Id : id, Permissions: permissions, Roles: roles);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        return _httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}
