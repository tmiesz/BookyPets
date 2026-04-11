using BookyPets.Api.Services;
using BookyPets.Application.Common.Interfaces;

namespace BookyPets.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddProblemDetails();
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentReaderProvider, CurrentReaderProvider>();

        return services;
    }
}
