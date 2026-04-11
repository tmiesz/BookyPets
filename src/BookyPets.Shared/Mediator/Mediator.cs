using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BookyPets.Shared.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();
        var tasks = handlers.Select(handler => handler.HandleAsync(notification, cancellationToken));

        await Task.WhenAll(tasks);
    }

    public Task PublishAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        var method = typeof(Mediator)
            .GetMethods()
            .Single(m => m.Name == nameof(PublishAsync) && m.IsGenericMethod)
            .MakeGenericMethod(notification.GetType());

        return (Task)method.Invoke(this, [notification, cancellationToken])!;
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

        var handlerType = typeof(IHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetRequiredService(handlerType);
        var handlerMehtod = handlerType.GetMethod(nameof(IHandler<,>.HandleAsync))!;

        HandlerDelegate<TResponse> pipeline = () => (Task<TResponse>)handlerMehtod.Invoke(handler, [request, cancellationToken])!;

        var behaviourType = typeof(IPipelineBehaviour<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviourMethod = behaviourType.GetMethod(nameof(IPipelineBehaviour<IRequest<TResponse>, TResponse>.HandleAsync))!;

        foreach (var behaviour in _serviceProvider.GetServices(behaviourType).Reverse())
        {
            var next = pipeline;
            pipeline = () => (Task<TResponse>)behaviourMethod.Invoke(behaviour, [request, next, cancellationToken])!;
        }

        return await pipeline();
    }
}
