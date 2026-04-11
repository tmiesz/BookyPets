using BookyPets.Domain.PetAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Pets.Queries.GetPet;

public record GetPetQuery(Guid PetId) : IRequest<Result<Pet>>;
