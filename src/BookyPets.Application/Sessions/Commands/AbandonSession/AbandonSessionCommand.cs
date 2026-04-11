using BookyPets.Application.Common.Authorization;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Commands.AbandonSession;

[Authorize(Permissions = "sessions:finish")]
public record AbandonSessionCommand(Guid SessionId) : IRequest<Result<Session>>;
