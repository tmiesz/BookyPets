using BookyPets.Application.Common.Authorization;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Commands.CompleteSession;

[Authorize(Permissions = "sessions:finish")]
public record CompleteSessionCommand(Guid SessionId, int PagesRead) : IRequest<Result<Session>>;
