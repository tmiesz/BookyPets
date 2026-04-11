namespace BookyPets.Shared.Mediator.Abstractions;

public interface IPipelineBehaviour<in TRequest> where TRequest : notnull
{
    Task HandleAsync(TRequest request, HandlerDelegate next, CancellationToken cancellationToken);
}

public interface IPipelineBehaviour<in TRequest, TResponse> where TRequest : notnull
{
    Task<TResponse> HandleAsync(TRequest request, HandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

public delegate Task HandlerDelegate();
public delegate Task<TResponse> HandlerDelegate<TResponse>();
