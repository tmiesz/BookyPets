using BookyPets.Application.Authentication.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Authentication.Commands;

public record RegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Result<AuthenticationResult>>;
