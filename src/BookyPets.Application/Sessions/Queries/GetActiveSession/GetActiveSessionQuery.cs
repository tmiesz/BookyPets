using BookyPets.Application.Common.Authorization;
using BookyPets.Application.Sessions.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Queries.GetActiveSession;


[Authorize(Permissions = "sessions:start")]
public record GetActiveSessionQuery : IRequest<Result<ActiveSessionResult?>>;
