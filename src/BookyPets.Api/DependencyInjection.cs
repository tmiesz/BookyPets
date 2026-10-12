using BookyPets.Api.Services;
using BookyPets.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BookyPets.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddProblemDetails();
        services.AddHttpContextAccessor();

        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(
                new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());

        services.AddScoped<ICurrentReaderProvider, CurrentReaderProvider>();

        return services;
    }
}
