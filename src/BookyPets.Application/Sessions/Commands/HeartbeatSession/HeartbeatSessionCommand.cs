using BookyPets.Application.Common.Authorization;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Commands.HeartbeatSession;

[Authorize(Permissions = "sessions:start")]
public record HeartbeatSessionCommand : IRequest<Result>;
