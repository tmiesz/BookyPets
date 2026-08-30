using BookyPets.Domain.PetAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Pets.Queries.GetPets;

public record GetPetsQuery(string? Search = null) : IRequest<Result<List<Pet>>>;
