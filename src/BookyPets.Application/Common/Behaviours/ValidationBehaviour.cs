using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using BookyPets.Shared.Validator;

namespace BookyPets.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IValidator<TRequest>? validator = null) : IPipelineBehaviour<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
{
    private readonly IValidator<TRequest>? _validator = validator;
    public async Task<TResponse> HandleAsync(TRequest request, HandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validator is null)
            return await next();

        var result = _validator.Validate(request);
        if (result.IsValid)
            return await next();

        var failure = result.Failures[0];
        var error = new Error(ErrorType.Validation, code: failure.PropertyName, description: failure.ErrorMessage);

        return (dynamic)error;
    }
}
