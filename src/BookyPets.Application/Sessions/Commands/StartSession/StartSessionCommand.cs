using BookyPets.Application.Common.Authorization;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Commands.StartSession;


[Authorize(Permissions = "sessions:start")]
public record StartSessionCommand(Guid ProgressId, Guid? PetId) : IRequest<Result<Session>>;
