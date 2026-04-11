using BookyPets.Application.Common.Authorization;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Pets.Commands.GiveExperience;

[Authorize(Roles = Role.Admin)]
public record GiveExperienceCommand(Guid PetId, int Experience) : IRequest<Result>;
