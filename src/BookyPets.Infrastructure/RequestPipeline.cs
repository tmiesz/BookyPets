using BookyPets.Infrastructure.Common.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BookyPets.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<EventualConsistencyMiddleware>();

        return builder;
    }
}
