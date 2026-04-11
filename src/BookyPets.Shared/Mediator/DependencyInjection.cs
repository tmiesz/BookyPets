using System.Reflection;
using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BookyPets.Shared.Mediator;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IMediator, Mediator>();

        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType &&
                            (i.GetGenericTypeDefinition() == typeof(IHandler<>) ||
                             i.GetGenericTypeDefinition() == typeof(IHandler<,>)))
                .Select(i => new { HandlerInterface = i, HandlerImplementation = t }));

        foreach (var handler in handlerTypes)
        {
            services.AddScoped(handler.HandlerInterface, handler.HandlerImplementation);
        }

        var notificationHandlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                .Select(i => new { HandlerInterface = i, HandlerImplementation = t }));

        foreach (var handler in notificationHandlerTypes)
        {
            services.AddScoped(handler.HandlerInterface, handler.HandlerImplementation);
        }

        var pipelineBehaviourTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && !t.IsGenericTypeDefinition)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType &&
                            (i.GetGenericTypeDefinition() == typeof(IPipelineBehaviour<>) ||
                            i.GetGenericTypeDefinition() == typeof(IPipelineBehaviour<,>)))
                .Select(i => new { BehaviourInterface = i, BehaviourImplementation = t }));

        foreach (var behaviour in pipelineBehaviourTypes)
        {
            services.AddScoped(behaviour.BehaviourInterface, behaviour.BehaviourImplementation);
        }

        return services;
    }
}
