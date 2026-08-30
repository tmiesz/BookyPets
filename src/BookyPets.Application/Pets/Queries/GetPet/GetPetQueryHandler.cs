using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.PetAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Pets.Queries.GetPet;

public class GetPetQueryHandler(IPetsRepository petsRepository) : IHandler<GetPetQuery, Result<Pet>>
{
    private readonly IPetsRepository _petsRepository = petsRepository;

    public async Task<Result<Pet>> HandleAsync(GetPetQuery query, CancellationToken cancellationToken = default)
    {
        var pet = await _petsRepository.GetPetAsync(query.PetId);

        if(pet is null)
            return new Error(ErrorType.NotFound, "PetNotFound");

        return pet;
    }
}
