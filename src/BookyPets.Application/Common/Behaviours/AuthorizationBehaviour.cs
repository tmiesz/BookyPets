using System.Reflection;
using BookyPets.Application.Common.Authorization;
using BookyPets.Application.Common.Interfaces;
using BookyPets.Application.Common.Models;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse>(ICurrentReaderProvider _currentReaderProvider)
    : IPipelineBehaviour<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
{
    public async Task<TResponse> HandleAsync(TRequest request, HandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        CurrentReader currentReader;
        try
        {
            currentReader = _currentReaderProvider.GetCurrentReader();
        }
        catch
        {
            return (dynamic)new Error(ErrorType.Unauthorized, "Unauthorized", "User is not authenticated");
        }


        var requiredPermissions = authorizationAttributes
            .SelectMany(authorizationAttributes => authorizationAttributes.Permissions?.Split(',') ?? [])
            .ToList();

        if (requiredPermissions.Except(currentReader.Permissions).Any())
        {
            return (dynamic)new Error(ErrorType.Unauthorized, "ReaderUnauthorized", "Reader is forbiddden from taking this action");
        }

        var requiredRoles = authorizationAttributes
            .SelectMany(authorizationAttributes => authorizationAttributes.Roles?.Split(',') ?? [])
            .ToList();

        if (requiredRoles.Except(currentReader.Roles).Any())
        {
            return (dynamic)new Error(ErrorType.Forbidden, "ReaderForbidden", "Reader is forbiddden from taking this action");
        }

        return await next();
    }
}
