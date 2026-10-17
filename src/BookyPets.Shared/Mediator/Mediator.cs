using System.Reflection;
using System.Runtime.ExceptionServices;
using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BookyPets.Shared.Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(notification, cancellationToken);
        }
    }

    public Task PublishAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        var method = typeof(Mediator)
            .GetMethods()
            .Single(m => m.Name == nameof(PublishAsync) && m.IsGenericMethod)
            .MakeGenericMethod(notification.GetType());

        return Invoke<Task>(method, this, [notification, cancellationToken]);
    }

    public async Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : class, IRequest
    {
        var handler = _serviceProvider.GetRequiredService<IHandler<TRequest>>();

        HandlerDelegate pipeline = () => handler.HandleAsync(request, cancellationToken);

        foreach (var behaviour in _serviceProvider.GetServices<IPipelineBehaviour<TRequest>>().Reverse())
        {
            var next = pipeline;
            pipeline = () => behaviour.HandleAsync(request, next, cancellationToken);
        }

        await pipeline();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = _serviceProvider.GetRequiredService(handlerType);
        var handlerMethod = handlerType.GetMethod(nameof(IHandler<,>.HandleAsync))!;

        HandlerDelegate<TResponse> pipeline = () => Invoke<Task<TResponse>>(handlerMethod, handler, [request, cancellationToken]);

        var behaviourType = typeof(IPipelineBehaviour<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviourMethod = behaviourType.GetMethod(nameof(IPipelineBehaviour<,>.HandleAsync))!;

        foreach (var behaviour in _serviceProvider.GetServices(behaviourType).Reverse())
        {
            var next = pipeline;
            pipeline = () => Invoke<Task<TResponse>>(behaviourMethod, behaviour, [request, next, cancellationToken]);
        }

        return await pipeline();
    }

    private static T Invoke<T>(MethodInfo method, object? target, object?[] arguments)
    {
        try
        {
            return (T)method.Invoke(target, arguments)!;
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }
}
