using BookyPets.Application.Authentication.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Authentication.Queries;

public record LoginQuery(string Email, string Password) : IRequest<Result<AuthenticationResult>>;
