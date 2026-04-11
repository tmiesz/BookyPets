using BookyPets.Application.Common.Authorization;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Commands.AcquirePet;

[Authorize(Permissions = "pets:acquire")]
public record AcquirePetCommand(Guid PetId) : IRequest<Result>;
