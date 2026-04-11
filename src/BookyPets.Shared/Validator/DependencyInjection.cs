using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BookyPets.Shared.Validator;

public static class DependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly)
    {
        var validatorTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetBaseTypes()
                    .Where(b => b.IsGenericType && b.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
                    .Select(b => new
                        {
                            ValidatorInterface = typeof(IValidator<>).MakeGenericType(b.GetGenericArguments()),
                            ValidatorImplementation = t
                        })
                    );

        foreach (var validator in validatorTypes)
            services.AddScoped(validator.ValidatorInterface, validator.ValidatorImplementation);

        return services;
    }

    private static IEnumerable<Type> GetBaseTypes(this Type type)
    {
        var current = type.BaseType;
        while (current != null && current != typeof(object))
        {
            yield return current;
            current = current.BaseType;
        }
    }
}
