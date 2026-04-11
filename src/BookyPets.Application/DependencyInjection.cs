using System.Reflection;
using BookyPets.Application.Common.Behaviours;
using BookyPets.Shared.Mediator;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Validator;
using Microsoft.Extensions.DependencyInjection;

namespace BookyPets.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(Assembly.GetExecutingAssembly());
        services.AddValidators(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IPipelineBehaviour<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehaviour<,>), typeof(AuthorizationBehaviour<,>));

        return services;
    }
}
